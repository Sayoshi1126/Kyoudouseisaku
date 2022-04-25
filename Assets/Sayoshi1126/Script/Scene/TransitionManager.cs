using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Script.Utilities.SceneDataPacks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Utilities.SceneTransition{
    public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
    {

        /// <summary>
        /// 蓋絵(トランジションアニメーションの管理コンポーネント)
        /// </summary>
        private MyEMTransition _transionComponent;
        /// <summary>
        /// 蓋絵のImage
        /// </summary>
        private RawImage _image;

        /// <summary>
        /// シーン遷移実行中であるか
        /// </summary>
        private bool _isRunning = false;

        public bool IsRunning {get {return _isRunning;} }

        /// <summary>
        /// トランジションアニメーションを終了させて良いか
        /// ReactivePropertyは普通の変数みたいに扱えるsubjectみたいなもの
        /// </summary>
        private ReactiveProperty<bool> CanEndTransition = new ReactiveProperty<bool>(false);

        /// <summary>
        /// 現在のシーン状況
        /// </summary>
        private GameScenes _currentGameScene;

        public GameScenes CurrentGameScene
        {
            get { return _currentGameScene; }
        }

        /// <summary>
        /// トランジション終了通知
        /// UnitはSubjectの中身に意味はなく、タイミングが重要な時に使われる
        /// </summary>
        private Subject<Unit> _onTransactionFinishedInternal = new Subject<Unit>();

        /// <summary>
        /// トランジションが終了しシーンが開始したことを通知する
        /// </summary>
        private Subject<Unit> _onTransitionAnimationFinishedSubject = new Subject<Unit>();

        private Subject<Unit> onAllSceneLoaded = new Subject<Unit>();

        public IObservable<Unit> OnScenesLoaded { get { return onAllSceneLoaded; } }

        public IObservable<Unit> OnTransitionAnimationFinished
        {
            get
            {
                if (_isRunning)
                {
                    //FirstOrDefaultはUniRxのフィルタリングの一つ
                    ///一番始めに到達したOnNextのみ流してObservableを完了させる
                    return _onTransitionAnimationFinishedSubject.FirstOrDefault();
                }
                else
                {
                    return Observable.Return(Unit.Default);
                }
            }
        }

        /// <summary>
        /// トランジションアニメーションを終了させる
        /// (AutoMove=falseを指定した際に呼び出す必要がある)
        /// </summary>
        public void Open()
        {
            ///ReactivePropertyは値を書き換えた時にOnNextが飛ぶ
            CanEndTransition.Value = true;
        }

        protected override void Awake()
        {
            base.Awake();

            //try catchでは例外判定を読み取る
            //tryに例外判定を書き、catchに例外判定後の処理を各
            try
            {
                _currentGameScene = (GameScenes)Enum.Parse(typeof(GameScenes), SceneManager.GetActiveScene().name, false);
            }
            catch
            {
                Debug.Log("現在のシーンの取得に失敗");
                _currentGameScene = GameScenes.TitleScene; //Debugシーンとかの場合は適当なシーンで埋めておく
            }
        }

        private void Start()
        {
            try
            {
                _currentGameScene = (GameScenes)Enum.Parse(typeof(GameScenes), SceneManager.GetActiveScene().name, false);
            }
            catch
            {
                Debug.Log("現在のシーンの取得に失敗");
                _currentGameScene = GameScenes.TitleScene; //Debugシーンとかの場合は適当なシーンで埋めておく
            }
                Initialize();

            //トランジションの終了を待機してゲームを開始するような設定の場合を想定して
            //初期化直後にシーン遷移完了通知を発行する(デバッグで任意のシーンからゲームを開始できるように)
            onAllSceneLoaded.OnNext(Unit.Default);
        }

        private void Initialize()
        {
            if(_transionComponent == null)
            {
                _transionComponent = GetComponent<MyEMTransition>();
                _transionComponent.onTransitionComplete.AddListener(
                    () => _onTransactionFinishedInternal.OnNext(Unit.Default));
            }
        }

        /// <summary>
        /// シーン遷移の実行
        /// </summary>
        /// <param name="nextScene"></param>
        /// <param name="data"></param>
        /// <param name="autoMove"></param>
        public void StartTransaction(
            GameScenes nextScene,
            SceneDataPack data,
            bool autoMove
            )
        {
            if (_isRunning) return;
            StartCoroutine(TransitionCoroutine(nextScene, data, autoMove));
        }

        /// <summary>
        /// シーン遷移処理の本体
        /// </summary>
        /// <param name="newxtScene"></param>
        /// <param name="data"></param>
        /// <param name="autoMove"></param>
        /// <returns></returns>
        private IEnumerator TransitionCoroutine(
            GameScenes nextScene,
            SceneDataPack data,
            bool autoMove)
        {
            //処理開始フラグセット
            _isRunning = true;
            //トランジションの自動遷移設定
            CanEndTransition.Value = autoMove;

            if(_transionComponent == null)
            {
                Initialize();
                yield return null;
            }

            //トランジション開始(蓋絵で画面を隠す)
            _transionComponent.FadeIn();

            //トランジションアニメーションが終了するのを待つ
            //yield return _onTransactionFinishedInternal.FirstOrDefault().ToYieldInstruction();
            yield return new WaitForSeconds(0.6f);

            //前のシーンから受け取った情報を登録
            SceneLoader.PreviousSceneData = data;

            yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

            yield return null;

            //現在のシーンを設定
            _currentGameScene = nextScene;

            //シーンロード完了の通知
            onAllSceneLoaded.OnNext(Unit.Default);

            if (!autoMove)
            {
                //自動遷移しない設定の場合はフラグがtrueに変化するまで待機(ToYieldInstructionはCompletedしてからでないとコルーチンを終了しない)
                //namespaceの中のclassにラムダ式があるとMonobehaviorが参照されないのでdelegateで書く
                //delegate (bool x) { return x; } == x => x
                yield return CanEndTransition.FirstOrDefault(delegate (bool x) { return x; }).ToYieldInstruction();
            }
            CanEndTransition.Value = false;

            //蓋絵を開く方のアニメーション開始
            //_transionComponent.FadeIn();
            _transionComponent.FadeOut();

            //蓋絵が開ききるのを待つ
            //yield return _onTransactionFinishedInternal.FirstOrDefault().ToYieldInstruction();
            yield return new WaitForSeconds(0.6f);

            //トランジションが全て完了したことを通知
            _onTransitionAnimationFinishedSubject.OnNext(Unit.Default);

            //終了
            _isRunning = false;
        }
    }
}
