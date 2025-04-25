using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using TeamTodayTextRPG;
using System.Threading;

namespace TeamTodayTextRPG
{
    class GameManager
    {


        /* 『효빈』
            Lazy<T> 형식의 싱글톤 방법입니다.
            인스턴스의 생성 시기를 직접 정할 수 있기 때문에 리소스 낭비를 줄일 수 있고, 스레드 안정성 문제도 해결 가능합니다. 
        */
        private static readonly Lazy<GameManager> lazyInstance = new Lazy<GameManager>(() => new GameManager());
        public static GameManager Instance => lazyInstance.Value;


        // 『효빈』GameManager 생성자 입니다.
        private GameManager()
        {
            Player = new Player();
            rand = new Random();

        }


        // 『효빈』GameMananger 클래스의 프로퍼티 입니다.
        public Player Player { get; set; }
        public Random rand { get; set; }

        public Monster BattleEnemy { get; set; }

        public SceneManager SceneManager => SceneManager.Instance;


    }
}