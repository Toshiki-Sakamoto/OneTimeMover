using System;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Stage;
using MoreMountains.Tools;
using UnityEngine;

namespace OneTripMover.Views.Stage
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] private StageId _stageId = new (1);
        
        private StageIntro _stageIntro;
        private IStageUseCase _stageUseCase;
        
        public void Initialize(StageId stageId)
        {
            _stageId = stageId;
        }
        
        private void OnGamePhaseWillEnter(GamePhaseWillEnterEvent evt)
        {
            if (_stageUseCase.GetCurrentStage() != _stageId) return;
            
            if (evt.NextPhase == GamePhase.Initialize)
            {
                _stageIntro.PlayIntro();
            }
        }

        private void Awake()
        {
            _stageIntro = GetComponentInChildren<StageIntro>(true);
            
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();

            var gamePhaseWillSubscriber = ServiceLocator.Resolve<ISubscriber<GamePhaseWillEnterEvent>>();
            gamePhaseWillSubscriber.SubscribeDisposable(OnGamePhaseWillEnter).AddTo(this);
        }
    }
}