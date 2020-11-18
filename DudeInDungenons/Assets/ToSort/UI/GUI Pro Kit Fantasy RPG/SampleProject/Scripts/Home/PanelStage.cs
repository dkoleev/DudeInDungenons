using FantasyRPG;

namespace ToSort.UI.GUI_Pro_Kit_Fantasy_RPG.SampleProject.Scripts.Home {
    public class PanelStage : PanelBase
    {
        public Navicontrol navicontrol;
    
        public void Click_Prev()
        {
            navicontrol.Prev();
        }

        public void Click_Next()
        {
            navicontrol.Next();
        }

        public void Click_Stage()
        {
            PlayManager.Instance.LoadScene(FantasyRPG.Data.scene_play);
        }
    }
}
