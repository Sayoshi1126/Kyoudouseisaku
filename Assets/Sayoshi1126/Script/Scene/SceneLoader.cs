using System;
using System.Collections;
using System.Collections.Generic;
using Script.Utilities.SceneDataPacks;
using Script.Utilities.SceneTransition;
using UniRx;
using UnityEngine;

namespace Script.Utilities
{
    public class SceneLoader
    {
        // Start is called before the first frame update

        /// <summary>
        /// 前のシーンから引き継いだデータ
        /// </summary>
        public static SceneDataPack PreviousSceneData;

        /// <summary>
        /// シーン遷移マネージャー
        /// </summary>
        private static TransitionManager _transitionManager;

        /// <summary>
        /// シーン遷移マネージャー
        /// 存在しない場合は生成する
        /// </summary>
        private static TransitionManager TransitionManager
        {
            get
            {
                if (_transitionManager != null) return _transitionManager;
                Initialize();
                return _transitionManager;
            }
        }

        /// <summary>
        /// トランジションマネージャーが存在しない場合に初期化する
        /// </summary>
        public static void Initialize()
        {
            if (TransitionManager.Instance == null)
            {
                var resource = Resources.Load("Prefabs/Utilities/TransitionCanvas");
                MonoBehaviour.Instantiate(resource);
            }
            _transitionManager = TransitionManager.Instance;
        }

        /// <summary>
        /// シーン遷移のトランジションが完了したことを通知する
        /// </summary>
        public static IObservable<Unit> OnTransitionFinished
        {
            get { return TransitionManager.OnTransitionAnimationFinished; }
        }

        /// <summary>
        /// シーンのロードが全て完了したことを通知する
        /// </summary>
        public static IObservable<Unit> OnScenesLoaded
        {
            get { return TransitionManager.OnScenesLoaded.FirstOrDefault(); }
        }

        /// <summary>
        /// トランジションアニメーションを終了させてゲームシーンを移す
        /// （AutoMoveにfalseを指定した際に実行する必要がある）
        /// </summary>
        public static void Open()
        {
            TransitionManager.Open();
        }

        /// <summary>
        ///　シーン遷移処理中か
        /// </summary>
        public static bool IsTransitionRunning
        {
            get { return TransitionManager.IsRunning; }
        }

        /// <summary>
        /// シーン遷移を行う
        /// </summary>
        /// <param name="scene">次のシーン</param>
        /// <param name="data">次のシーンへ引き継ぐデータ</param>
        /// <param name="additiveLoadScenes">追加でロードするシーン</param>
        /// <param name="autoMove">トランジションアニメーションを自動的に完了させるか
        ///                        falseの場合はOpen()を実行しないとトランジションが終了しない</param>
        public static void LoadScene(GameScenes scene,
            SceneDataPack data = null,
            GameScenes[] additiveLoadScenes = null,
            bool autoMove = true)
        {
            if (data == null)
            {
                //引き継ぐデータが未指定の場合はシーン情報のみを詰める
                data = new DefaultSceneDataPack(TransitionManager.CurrentGameScene);
            }
            TransitionManager.StartTransaction(scene, data, autoMove);
        }
    }
}