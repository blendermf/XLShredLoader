using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameManagement;

namespace XLShredLib {
    public class ExtraStateModInfo {
        public ExtraStateModInfo() {
            ExtraAvailableTransitions = new List<Type>();
        }

        public List<Type> ExtraAvailableTransitions { get; set; }

        public bool CanDoTransitionToExtra(Type targetState) {
            return this.ExtraAvailableTransitions.Contains(targetState);
        }
    }

    public class PauseStateModInfo : ExtraStateModInfo {
        private static PauseStateModInfo instance = null;
        private static readonly object padlock = new object();

        public PauseStateModInfo() : base() {
            base.ExtraAvailableTransitions.Add(typeof(ModSettingsState));
        }

        public static PauseStateModInfo Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new PauseStateModInfo();
                    }
                    return instance;
                }
            }
        }
    }

    public class PlayStateModInfo : ExtraStateModInfo {
        private static PlayStateModInfo instance = null;
        private static readonly object padlock = new object();

        public PlayStateModInfo() : base() {
            base.ExtraAvailableTransitions.Add(typeof(ModSettingsState));
        }

        public static PlayStateModInfo Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new PlayStateModInfo();
                    }
                    return instance;
                }
            }
        }
    }

    public class GearSelectionStateModInfo : ExtraStateModInfo {
        private static GearSelectionStateModInfo instance = null;
        private static readonly object padlock = new object();

        public GearSelectionStateModInfo() : base() {
        }

        public static GearSelectionStateModInfo Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new GearSelectionStateModInfo();
                    }
                    return instance;
                }
            }
        }
    }

    public class LevelSelectionStateModInfo : ExtraStateModInfo {
        private static LevelSelectionStateModInfo instance = null;
        private static readonly object padlock = new object();

        public LevelSelectionStateModInfo() : base() {
        }

        public static LevelSelectionStateModInfo Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new LevelSelectionStateModInfo();
                    }
                    return instance;
                }
            }
        }
    }

    public class PinMovementStateModInfo : ExtraStateModInfo {
        private static PinMovementStateModInfo instance = null;
        private static readonly object padlock = new object();

        public PinMovementStateModInfo() : base() {
        }

        public static PinMovementStateModInfo Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new PinMovementStateModInfo();
                    }
                    return instance;
                }
            }
        }
    }

    public class ReplayStateModInfo : ExtraStateModInfo {
        private static ReplayStateModInfo instance = null;
        private static readonly object padlock = new object();

        public ReplayStateModInfo() : base() {
        }

        public static ReplayStateModInfo Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new ReplayStateModInfo();
                    }
                    return instance;
                }
            }
        }
    }

    public class TutorialStateModInfo : ExtraStateModInfo {
        private static TutorialStateModInfo instance = null;
        private static readonly object padlock = new object();

        public TutorialStateModInfo() : base() {
        }

        public static TutorialStateModInfo Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new TutorialStateModInfo();
                    }
                    return instance;
                }
            }
        }
    }
}
