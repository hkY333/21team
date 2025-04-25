using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamTodayTextRPG
{
    class TRPG()
    {
        static void Main(string[] args)
        {
            // 『효빈』VIEW_TYPE의 변수생성
            VIEW_TYPE currentView = VIEW_TYPE.MAIN;

            /* 『효빈』
                게임 전체를 관리해줄 GameManager 인스턴스
                이후 모든 메소드의 접근을 gm을 통해 행합니다!!
            */
            var gm = GameManager.Instance;
            var sm = SceneManager.Instance;
            var dm = DataManager.Instance;


            sm.Intro();


            // 『효빈』스테이트 머신
            while (true)
            {
                sm.SwitchScene(currentView);


                //『효빈』선택지 입력 시 다음 화면으로의 전환
                currentView = sm.CurrentViewer.NextView(sm.InputAction(sm.CurrentViewer.StartIndex, sm.CurrentViewer.EndIndex));
            }
        }
    }
}