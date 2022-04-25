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
        /// �O�̃V�[����������p�����f�[�^
        /// </summary>
        public static SceneDataPack PreviousSceneData;

        /// <summary>
        /// �V�[���J�ڃ}�l�[�W���[
        /// </summary>
        private static TransitionManager _transitionManager;

        /// <summary>
        /// �V�[���J�ڃ}�l�[�W���[
        /// ���݂��Ȃ��ꍇ�͐�������
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
        /// �g�����W�V�����}�l�[�W���[�����݂��Ȃ��ꍇ�ɏ���������
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
        /// �V�[���J�ڂ̃g�����W�V�����������������Ƃ�ʒm����
        /// </summary>
        public static IObservable<Unit> OnTransitionFinished
        {
            get { return TransitionManager.OnTransitionAnimationFinished; }
        }

        /// <summary>
        /// �V�[���̃��[�h���S�Ċ����������Ƃ�ʒm����
        /// </summary>
        public static IObservable<Unit> OnScenesLoaded
        {
            get { return TransitionManager.OnScenesLoaded.FirstOrDefault(); }
        }

        /// <summary>
        /// �g�����W�V�����A�j���[�V�������I�������ăQ�[���V�[�����ڂ�
        /// �iAutoMove��false���w�肵���ۂɎ��s����K�v������j
        /// </summary>
        public static void Open()
        {
            TransitionManager.Open();
        }

        /// <summary>
        ///�@�V�[���J�ڏ�������
        /// </summary>
        public static bool IsTransitionRunning
        {
            get { return TransitionManager.IsRunning; }
        }

        /// <summary>
        /// �V�[���J�ڂ��s��
        /// </summary>
        /// <param name="scene">���̃V�[��</param>
        /// <param name="data">���̃V�[���ֈ����p���f�[�^</param>
        /// <param name="additiveLoadScenes">�ǉ��Ń��[�h����V�[��</param>
        /// <param name="autoMove">�g�����W�V�����A�j���[�V�����������I�Ɋ��������邩
        ///                        false�̏ꍇ��Open()�����s���Ȃ��ƃg�����W�V�������I�����Ȃ�</param>
        public static void LoadScene(GameScenes scene,
            SceneDataPack data = null,
            GameScenes[] additiveLoadScenes = null,
            bool autoMove = true)
        {
            if (data == null)
            {
                //�����p���f�[�^�����w��̏ꍇ�̓V�[�����݂̂��l�߂�
                data = new DefaultSceneDataPack(TransitionManager.CurrentGameScene);
            }
            TransitionManager.StartTransaction(scene, data, autoMove);
        }
    }
}