using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameManagement;

namespace XLShredLib {
    class ModSettingsState : GameState {
        public ModSettingsState() {
            this.availableTransitions = new Type[]
            {
                typeof(PauseState),
                typeof(PlayState)
            };
        }

        public override void OnEnter() {
            // make mod settings menu object active
        }

        public override void OnUpdate() {
            if (PlayerController.Instance.inputController.player.GetButtonDown("B")) {
                base.RequestTransitionTo(typeof(PauseState));
            }
        }

        public override void OnExit() {
            // make mod settings menu object inactive
        }
    }
}
