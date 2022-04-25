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
        /// �W�G(�g�����W�V�����A�j���[�V�����̊Ǘ��R���|�[�l���g)
        /// </summary>
        private MyEMTransition _transionComponent;
        /// <summary>
        /// �W�G��Image
        /// </summary>
        private RawImage _image;

        /// <summary>
        /// �V�[���J�ڎ��s���ł��邩
        /// </summary>
        private bool _isRunning = false;

        public bool IsRunning {get {return _isRunning;} }

        /// <summary>
        /// �g�����W�V�����A�j���[�V�������I�������ėǂ���
        /// ReactiveProperty�͕��ʂ̕ϐ��݂����Ɉ�����subject�݂����Ȃ���
        /// </summary>
        private ReactiveProperty<bool> CanEndTransition = new ReactiveProperty<bool>(false);

        /// <summary>
        /// ���݂̃V�[����
        /// </summary>
        private GameScenes _currentGameScene;

        public GameScenes CurrentGameScene
        {
            get { return _currentGameScene; }
        }

        /// <summary>
        /// �g�����W�V�����I���ʒm
        /// Unit��Subject�̒��g�ɈӖ��͂Ȃ��A�^�C�~���O���d�v�Ȏ��Ɏg����
        /// </summary>
        private Subject<Unit> _onTransactionFinishedInternal = new Subject<Unit>();

        /// <summary>
        /// �g�����W�V�������I�����V�[�����J�n�������Ƃ�ʒm����
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
                    //FirstOrDefault��UniRx�̃t�B���^�����O�̈��
                    ///��Ԏn�߂ɓ��B����OnNext�̂ݗ�����Observable������������
                    return _onTransitionAnimationFinishedSubject.FirstOrDefault();
                }
                else
                {
                    return Observable.Return(Unit.Default);
                }
            }
        }

        /// <summary>
        /// �g�����W�V�����A�j���[�V�������I��������
        /// (AutoMove=false���w�肵���ۂɌĂяo���K�v������)
        /// </summary>
        public void Open()
        {
            ///ReactiveProperty�͒l����������������OnNext�����
            CanEndTransition.Value = true;
        }

        protected override void Awake()
        {
            base.Awake();

            //try catch�ł͗�O�����ǂݎ��
            //try�ɗ�O����������Acatch�ɗ�O�����̏������e
            try
            {
                _currentGameScene = (GameScenes)Enum.Parse(typeof(GameScenes), SceneManager.GetActiveScene().name, false);
            }
            catch
            {
                Debug.Log("���݂̃V�[���̎擾�Ɏ��s");
                _currentGameScene = GameScenes.TitleScene; //Debug�V�[���Ƃ��̏ꍇ�͓K���ȃV�[���Ŗ��߂Ă���
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
                Debug.Log("���݂̃V�[���̎擾�Ɏ��s");
                _currentGameScene = GameScenes.TitleScene; //Debug�V�[���Ƃ��̏ꍇ�͓K���ȃV�[���Ŗ��߂Ă���
            }
                Initialize();

            //�g�����W�V�����̏I����ҋ@���ăQ�[�����J�n����悤�Ȑݒ�̏ꍇ��z�肵��
            //����������ɃV�[���J�ڊ����ʒm�𔭍s����(�f�o�b�O�ŔC�ӂ̃V�[������Q�[�����J�n�ł���悤��)
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
        /// �V�[���J�ڂ̎��s
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
        /// �V�[���J�ڏ����̖{��
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
            //�����J�n�t���O�Z�b�g
            _isRunning = true;
            //�g�����W�V�����̎����J�ڐݒ�
            CanEndTransition.Value = autoMove;

            if(_transionComponent == null)
            {
                Initialize();
                yield return null;
            }

            //�g�����W�V�����J�n(�W�G�ŉ�ʂ��B��)
            _transionComponent.FadeIn();

            //�g�����W�V�����A�j���[�V�������I������̂�҂�
            //yield return _onTransactionFinishedInternal.FirstOrDefault().ToYieldInstruction();
            yield return new WaitForSeconds(0.6f);

            //�O�̃V�[������󂯎��������o�^
            SceneLoader.PreviousSceneData = data;

            yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

            yield return null;

            //���݂̃V�[����ݒ�
            _currentGameScene = nextScene;

            //�V�[�����[�h�����̒ʒm
            onAllSceneLoaded.OnNext(Unit.Default);

            if (!autoMove)
            {
                //�����J�ڂ��Ȃ��ݒ�̏ꍇ�̓t���O��true�ɕω�����܂őҋ@(ToYieldInstruction��Completed���Ă���łȂ��ƃR���[�`�����I�����Ȃ�)
                //namespace�̒���class�Ƀ����_���������Monobehavior���Q�Ƃ���Ȃ��̂�delegate�ŏ���
                //delegate (bool x) { return x; } == x => x
                yield return CanEndTransition.FirstOrDefault(delegate (bool x) { return x; }).ToYieldInstruction();
            }
            CanEndTransition.Value = false;

            //�W�G���J�����̃A�j���[�V�����J�n
            //_transionComponent.FadeIn();
            _transionComponent.FadeOut();

            //�W�G���J������̂�҂�
            //yield return _onTransactionFinishedInternal.FirstOrDefault().ToYieldInstruction();
            yield return new WaitForSeconds(0.6f);

            //�g�����W�V�������S�Ċ����������Ƃ�ʒm
            _onTransitionAnimationFinishedSubject.OnNext(Unit.Default);

            //�I��
            _isRunning = false;
        }
    }
}
