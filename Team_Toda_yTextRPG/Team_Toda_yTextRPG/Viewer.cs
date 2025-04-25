using System;
using System.Numerics;
using TeamTodayTextRPG;
using static TeamTodayTextRPG.Characterclass;

namespace TeamTodayTextRPG
{
    // 뷰어 화면 타입을 정의하는 열거형
    // 가나다라
    public enum VIEW_TYPE
    {
        MAIN,           // 게임 시작 화면
        STATUS,         // 상태 보기 화면
        INVENTORY,      // 인벤토리 화면
        EQUIP,          // 장비 화면
        SHOP,           // 상점 화면
        PURCHASE,       // 아이템 구매 화면
        SALE,           // 아이템 판매 화면
        DUNGEON,        // 던전 화면
        DUNGEONCLEAR,   // 던전 클리어 화면
        REST,           // 휴식 화면
        BATTLE,         // 전투 화면
        MONSTER         // 몬스터 화면
    }

    // 모든 뷰어 클래스의 부모가 되는 추상 클래스
    public abstract class Viewer
    {
        public int StartIndex { get; set; }  // 화면에서 입력 가능한 시작 값
        public int EndIndex { get; set; }  // 화면에서 입력 가능한 끝 값
        public int DungeonCode { get; set; }// 던전 코드 (사용할 경우)

        protected Player Player => GameManager.Instance.Player;
        protected Character Character => Player.Character;

        protected int GetInput()
        {
            return SceneManager.Instance.InputAction(StartIndex, EndIndex);
        }


        // 각 화면에서의 구체적인 액션을 구현하는 추상 메서드
        public abstract void ViewAction();

        // 입력에 따라 다음 화면을 반환하는 추상 메서드
        public abstract VIEW_TYPE NextView(int choiceNum);
    }
    public class MainViewer : Viewer
    {
        public MainViewer()
        {
            StartIndex = 1;
            EndIndex = 7;
        }
        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("메인 화면");
            Console.WriteLine("====================");
            Console.WriteLine("1. 플레이어 상태 보기");
            Console.WriteLine("2. 인벤토리 보기");
            Console.WriteLine("3. 장비 보기");
            Console.WriteLine("4. 상점");
            Console.WriteLine("5. 던전");
            Console.WriteLine("6. 휴식");
            Console.WriteLine("7. 게임 종료");

            int input = GetInput();
            VIEW_TYPE nextView = NextView(input);
            SceneManager.Instance.SwitchScene(nextView);
        }
        // NextView 메서드 구현
        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 1:
                    return VIEW_TYPE.STATUS;
                case 2:
                    return VIEW_TYPE.INVENTORY;
                case 3:
                    return VIEW_TYPE.EQUIP;
                case 4:
                    return VIEW_TYPE.SHOP;
                case 5:
                    return VIEW_TYPE.DUNGEON;
                case 6:
                    return VIEW_TYPE.REST;
                case 7:
                    Console.WriteLine("게임을 종료합니다...");
                    Environment.Exit(0);
                    return VIEW_TYPE.MAIN;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return VIEW_TYPE.MAIN;
            }
        }

    }


    public class StatusViewer : Viewer
    {
        public StatusViewer()
        {
            StartIndex = 1;
            EndIndex = 1;
        }
        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("플레이어 상태 보기");
            Console.WriteLine("====================");


            // 플레이어의 상태를 출력
            Console.WriteLine($"직업: {Character.Jobname}");
            Console.WriteLine($"체력: {Character.Hp}/{Character.MaxHp}");
            Console.WriteLine($"마나: {Character.Mp}/{Character.MaxMp}");
            Console.WriteLine($"공격력: {Character.Attack} (+{Character.PlusAtk}) = {Character.TotalAtk}");
            Console.WriteLine($"방어력: {Character.Defence} (+{Character.PlusDef}) = {Character.TotalDef}");
            Console.WriteLine($"회피율: {Character.Dodge} (+{Character.PlusDodge}) = {Character.TotalDodge}");
            Console.WriteLine($"소지금: {Player.Gold}G");
            Console.WriteLine($"액티브 스킬: {Character.ActskillName}");
            Console.WriteLine($"패시브 스킬: {Character.PasskillName} (레벨 {Character.PassiveSkillLevel}/{Character.MaxPassiveSkillLevel})");

            Console.WriteLine("====================");
            Console.WriteLine("1. 메인으로 돌아가기");

            VIEW_TYPE nextView = NextView(SceneManager.Instance.InputAction(StartIndex, EndIndex));
            SceneManager.Instance.SwitchScene(nextView);
        }

        public override VIEW_TYPE NextView(int input)
        {
            if (input == 1)
            {
                // 메인 화면으로 돌아가기
                return VIEW_TYPE.MAIN;
            }
            else
            {
                // 잘못된 입력 처리
                return VIEW_TYPE.STATUS; // 기본적으로 현재 상태 화면 유지
            }
        }
    }


    public class InventoryViewer : Viewer
    {
        public InventoryViewer()
        {
            StartIndex = 1;
            EndIndex = 2;
        }
        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("====================");

            var player = GameManager.Instance.Player;

            // InventoryItems 목록이나 Item 클래스가 변경될 경우 수정 필요
            for (int i = 0; i < GameManager.Instance.Player.Bag.Count; i++)
            {

                Console.WriteLine($"{i + 1}. {DataManager.Instance.ItemDB.List[GameManager.Instance.Player.Bag[i]][1]} - 공격력: {DataManager.Instance.ItemDB.List[GameManager.Instance.Player.Bag[i]][2]} / 방어력: {DataManager.Instance.ItemDB.List[GameManager.Instance.Player.Bag[i]][3]}");
            }

            Console.WriteLine("====================");
            Console.WriteLine("1. 아이템 사용");
            Console.WriteLine("2. 메인으로 돌아가기");

            VIEW_TYPE nextView = NextView(SceneManager.Instance.InputAction(StartIndex, EndIndex));
            SceneManager.Instance.SwitchScene(nextView);
        }
        // NextView 메서드 구현
        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 1:
                    // 아이템 사용 화면으로 이동
                    return VIEW_TYPE.INVENTORY;  // 아이템 사용 화면
                case 2:
                    // 메인 화면으로 돌아가기
                    return VIEW_TYPE.MAIN;  // 메인 화면으로 전환
                default:
                    // 잘못된 입력 처리
                    Console.WriteLine("잘못된 입력입니다.");
                    return VIEW_TYPE.INVENTORY;  // 잘못된 입력 시 인벤토리 화면을 다시 표시
            }
        }
    }


    public class EquipViewer : Viewer
    {
        public EquipViewer()
        {
            StartIndex = 0;
            EndIndex = 0; // Bag이 생기면 EndIndex = Bag.Count;로 변경
        }
        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("장비");
            Console.WriteLine("====================");

            var player = GameManager.Instance.Player;
            var character = player.Character;

            Console.WriteLine($"직업: {character.Jobname}");
            Console.WriteLine($"총 공격력: {character.TotalAtk} (기본: {character.Attack} + 추가: {character.PlusAtk})");
            Console.WriteLine($"총 방어력: {character.TotalDef} (기본: {character.Defence} + 추가: {character.PlusDef})");
            Console.WriteLine($"총 회피율: {character.TotalDodge} (기본: {character.Dodge} + 추가: {character.PlusDodge})");

            Console.WriteLine("====================");
            for (int i = 0; i < GameManager.Instance.Player.Bag.Count; i++)
            {

                Console.WriteLine($"{i + 1}. {DataManager.Instance.ItemDB.List[GameManager.Instance.Player.Bag[i]][1]} - 공격력: {DataManager.Instance.ItemDB.List[GameManager.Instance.Player.Bag[i]][2]} / 방어력: {DataManager.Instance.ItemDB.List[GameManager.Instance.Player.Bag[i]][3]}");
            }
            Console.WriteLine("====================");
            Console.WriteLine("1~n. 장비 변경");
            Console.WriteLine("0. 메인으로 돌아가기");


            VIEW_TYPE nextView = NextView(SceneManager.Instance.InputAction(StartIndex, EndIndex));
            SceneManager.Instance.SwitchScene(nextView);
        }

        public override VIEW_TYPE NextView(int input)
        {
            if (input == 0) { return VIEW_TYPE.INVENTORY; }
            else if (input > 0 && input <= GameManager.Instance.Player.Bag.Count)
            {
                GameManager.Instance.Player.EquipItem(input);
                return VIEW_TYPE.EQUIP;
            }
            else
            {
                // 잘못된 입력 처리
                Console.WriteLine("잘못된 입력입니다.");
                return VIEW_TYPE.EQUIP;  // 잘못된 입력 시 장비 화면을 다시 표시
            }
        }
    }

    public class ShopViewer : Viewer
    {
        public ShopViewer()
        {
            StartIndex = 1;
            EndIndex = 3;
        }
        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("====================");

            Console.WriteLine($"플레이어 금액: {GameManager.Instance.Player.Gold}G");

            Console.WriteLine("상점에서 판매하는 아이템:");

            for (int i = 0; i < DataManager.Instance.ItemDB.List.Count; i++)
            {
                var item = DataManager.Instance.ItemDB.List[i];

                // 배열 길이 초과 예외 방지
                if (item.Length > 7)
                {
                    Console.WriteLine($"{i + 1}. {item[1]} - {item[7]}G");
                }
                else
                {
                    Console.WriteLine($"{i + 1}. [아이템 정보 부족]");
                }
            }

            Console.WriteLine("====================");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("3. 메인으로 돌아가기");

            int input = GetInput();
            VIEW_TYPE nextView = NextView(input);
            SceneManager.Instance.SwitchScene(nextView);
        }

        // NextView 메서드 구현
        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 1:
                    // 아이템 구매 화면으로 이동
                    return VIEW_TYPE.PURCHASE;
                case 2:
                    // 아이템 판매 화면으로 이동
                    return VIEW_TYPE.SALE;
                case 3:
                    // 메인 화면으로 돌아가기
                    return VIEW_TYPE.MAIN;
                default:
                    // 잘못된 입력 처리
                    Console.WriteLine("잘못된 입력입니다.");
                    return VIEW_TYPE.SHOP;  // 다시 상점 화면을 보여줌
            }
        }
    }

    public class PurchaseViewer : Viewer
    {
        public PurchaseViewer()
        {
            StartIndex = 0;
            EndIndex = DataManager.Instance.ItemDB.List.Count; // 아이템 개수만큼
        }

        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("아이템 구매");
            Console.WriteLine("====================");

            var player = GameManager.Instance.Player;
            Console.WriteLine($"플레이어 금액: {player.Gold}G");

            Console.WriteLine("구매할 아이템을 선택하세요:");
            for (int i = 0; i < DataManager.Instance.ItemDB.List.Count; i++)
            {
                var item = DataManager.Instance.ItemDB.List[i];
                if (item.Length > 7)
                    Console.WriteLine($"{i + 1}. {item[1]} - {item[7]}G");
                else
                    Console.WriteLine($"{i + 1}. [아이템 정보 부족]");
            }

            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("9. 판매 화면으로");

            int input = GetInput(); // 헬퍼 메서드 사용

            if (input == 0 || input == 9)
            {
                SceneManager.Instance.SwitchScene(NextView(input));
            }
            else if (input > 0 && input <= DataManager.Instance.ItemDB.List.Count)
            {
                var item = DataManager.Instance.ItemDB.List[input - 1];
                if (item.Length > 7 && player.Gold >= int.Parse(item[7]))
                {
                    player.Gold -= int.Parse(item[7]);
                    player.InputBag(int.Parse(item[0]), VIEW_TYPE.PURCHASE);
                    Console.WriteLine($"{item[1]} 아이템을 구매했습니다.");
                }
                else
                {
                    Console.WriteLine("금액이 부족하거나 잘못된 아이템입니다.");
                }

                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
                SceneManager.Instance.SwitchScene(VIEW_TYPE.SHOP);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
                SceneManager.Instance.SwitchScene(VIEW_TYPE.PURCHASE);
            }
        }

        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 0: return VIEW_TYPE.SHOP;  // 돌아가기: SHOP 화면으로 돌아갑니다.
                case 9: return VIEW_TYPE.SALE;  // 판매 화면으로 이동
                default: return VIEW_TYPE.PURCHASE; // 잘못된 입력: 다시 구매 화면으로 돌아갑니다.
            }
        }
    }



    public class SaleViewer : Viewer
    {
        public SaleViewer()
        {
            StartIndex = 0;
            EndIndex = 0; // Bag이 생기면 EndIndex = Bag.Count;
        }
        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("아이템 판매");
            Console.WriteLine("====================");

            var player = GameManager.Instance.Player;
            //var playerItems = player.GetInventoryItems();  // 플레이어 인벤토리에서 아이템 목록 가져오기

            Console.WriteLine("판매할 아이템을 선택하세요:");
            for (int i = 0; i < GameManager.Instance.Player.Bag.Count; i++)
            {
                var item = GameManager.Instance.Player.Bag[i];
                Console.WriteLine($"{i + 1}. {DataManager.Instance.ItemDB.List[item][1]} - {DataManager.Instance.ItemDB.List[item][7]}G");
            }

            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("9. 구매 화면으로");

            int input = SceneManager.Instance.InputAction(StartIndex, EndIndex);

            if (input == 0)
            {
                NextView(input);  // 상점 화면으로 돌아가기
            }
            else if (input == 9)
            {
                // 9번을 선택하면 구매 화면으로 이동
                SceneManager.Instance.SwitchScene(VIEW_TYPE.PURCHASE);
            }
            else if (input > 0 && input <= GameManager.Instance.Player.Bag.Count)
            {
                // 아이템 판매 처리
                var item = GameManager.Instance.Player.Bag[input - 1];
                player.Gold += int.Parse(DataManager.Instance.ItemDB.List[item][7]);  // 85%로 가격 처리할 것 매매가
                player.RemoveBag(item, VIEW_TYPE.SALE);
                Console.WriteLine($"{DataManager.Instance.ItemDB.List[item][1]} 아이템을 판매했습니다.");
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }

            SceneManager.Instance.SwitchScene(VIEW_TYPE.SHOP); // 상점으로 돌아가기
        }

        // NextView 메서드 구현
        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 0:
                    // 돌아가기: 상점 화면으로 이동
                    return VIEW_TYPE.SHOP;
                case 9:
                    // 구매 화면으로 이동
                    return VIEW_TYPE.PURCHASE;
                default:
                    return VIEW_TYPE.SHOP;
            }
        }
    }



    public class DungeonViewer : Viewer
    {
        protected Dungeon dungeon;
        protected Monster monster;

        public DungeonViewer()
        {
            dungeon = new Dungeon(0, "기본던전", 0, 0, 1, DUNGEON_DIFF.Easy);
            monster = new Slime();  // 기본 몬스터 설정

            StartIndex = 1;
            EndIndex = 2;
        }

        public DungeonViewer(Dungeon dungeon, Monster monster)
        {
            this.dungeon = dungeon;
            this.monster = monster;

            StartIndex = 1;
            EndIndex = 2;
        }

        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("던전");
            Console.WriteLine("====================");

            Console.WriteLine($"던전 이름: {dungeon.Name}");
            Console.WriteLine($"던전 코드: {dungeon.Code}");
            Console.WriteLine($"난이도: {dungeon.Diff}");
            Console.WriteLine($"추천 레벨: {dungeon.DefLevel}");
            Console.WriteLine($"기본 보상: {dungeon.Reward}G");
            Console.WriteLine($"경험치: {dungeon.Exp}Exp");

            // 몬스터 출력
            Console.WriteLine($"몬스터 등장! 이름: {monster.Name}");

            if (monster.Grade == MONSTER_GRADE.BOSS)
            {
                Console.WriteLine("\n[보스 효과] 플레이어 능력치가 10% 감소합니다!");
                var character = GameManager.Instance.Player.Character;
                character.Attack = (int)(character.Attack * 0.9);
                character.Defence = (int)(character.Defence * 0.9);
            }

            Console.WriteLine("====================");
            Console.WriteLine("1. 던전 입장 (전투 시작)");
            Console.WriteLine("2. 메인으로 돌아가기");

            int input = GetInput(); // 사용자 입력 받기
            VIEW_TYPE nextView = NextView(input);
            SceneManager.Instance.SwitchScene(nextView); // 화면 전환
        }

        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 1:
                    // 전투 몬스터 등록
                    GameManager.Instance.BattleEnemy = monster; // 몬스터 설정
                    return VIEW_TYPE.BATTLE;  // 전투 화면으로 이동
                case 2:
                    return VIEW_TYPE.MAIN; // 메인 화면으로 돌아가기
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return VIEW_TYPE.DUNGEON; // 기본적으로 던전 화면 유지
            }
        }
    }


    public class DungeonClearViewer : Viewer
    {
        protected Player player;
        protected Dungeon dungeon;

        public DungeonClearViewer()
        {
            player = GameManager.Instance.Player;
            dungeon = new Dungeon(0, "기본던전", 0, 0, 1, DUNGEON_DIFF.Easy);

            StartIndex = 1;
            EndIndex = 1;
        }

        public DungeonClearViewer(Player player, Dungeon dungeon)
        {
            this.player = player;
            this.dungeon = dungeon;

            StartIndex = 1;
            EndIndex = 1;
        }

        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("던전 클리어");
            Console.WriteLine("====================");

            var character = player.Character;

            if (character.Hp <= 0)
            {
                Console.WriteLine("플레이어가 쓰러졌습니다! 던전 클리어 실패!");
            }
            else
            {
                Console.WriteLine($"축하합니다! 던전을 클리어했습니다!");
                Console.WriteLine($"보상: {dungeon.Reward}G");
                Console.WriteLine($"경험치: {dungeon.Exp}Exp");

                // 보상 처리
                player.Gold += dungeon.Reward;
                //character.Exp += dungeon.Exp; 캐릭터 경험치 필요

                // 보스 효과 복구
                character.Attack = (int)(character.Attack / 0.9);
                character.Defence = (int)(character.Defence / 0.9);
                Console.WriteLine("\n[보스 효과] 플레이어 능력치 감소 효과가 복구되었습니다!");
            }

            Console.WriteLine("====================");
            Console.WriteLine("1. 메인으로 돌아가기");

            int input = GetInput();
            VIEW_TYPE nextView = NextView(input);
            SceneManager.Instance.SwitchScene(nextView);
        }

        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 1:
                    return VIEW_TYPE.MAIN;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return VIEW_TYPE.DUNGEONCLEAR;
            }
        }
    }



    public class RestViewer : Viewer
    {
        public RestViewer()
        {
            StartIndex = 1;
            EndIndex = 2;
        }
        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine("휴식");
            Console.WriteLine("====================");

            var character = GameManager.Instance.Player.Character;
            Console.WriteLine($"체력 회복: {character.Hp}/{character.MaxHp}");

            Console.WriteLine("휴식을 취하시겠습니까?");
            Console.WriteLine("1. 휴식");
            Console.WriteLine("2. 메인으로 돌아가기");

            VIEW_TYPE nextView = NextView(SceneManager.Instance.InputAction(StartIndex, EndIndex));
            SceneManager.Instance.SwitchScene(nextView);
        }

        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 1:
                    var character = GameManager.Instance.Player.Character;
                    character.Hp = character.MaxHp; // 체력 회복 처리
                    Console.WriteLine("휴식을 취했습니다.");
                    return VIEW_TYPE.MAIN;
                case 2:
                    return VIEW_TYPE.MAIN;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return VIEW_TYPE.REST;
            }
        }
    }


    public class BattleViewer : Viewer
    {
        public BattleViewer()
        {
            StartIndex = 1;
            EndIndex = 2;
        }

        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine(" 전투 시작 ");
            Console.WriteLine("====================");

            var character = GameManager.Instance.Player.Character;
            var enemy = GameManager.Instance.BattleEnemy;

            Console.WriteLine($"플레이어 체력: {character.Hp}/{character.MaxHp}");
            Console.WriteLine($"적 몬스터 체력: {enemy.Hp}/{enemy.MaxHp}");

            Console.WriteLine("====================");
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 도망");

            int input = GetInput();
            VIEW_TYPE nextView = NextView(input);
            SceneManager.Instance.SwitchScene(nextView);
        }

        public override VIEW_TYPE NextView(int input)
        {
            var character = GameManager.Instance.Player.Character;
            var enemy = GameManager.Instance.BattleEnemy;

            switch (input)
            {
                case 1:
                    // 플레이어 공격
                    int playerDamage = character.TotalAtk - enemy.Def;
                    if (playerDamage < 0) playerDamage = 0;
                    enemy.ManageHp(-playerDamage);
                    Console.WriteLine($"\n플레이어가 {enemy.Name}에게 {playerDamage}의 데미지를 입혔습니다!");

                    // 적이 죽었는지 확인
                    if (enemy.Hp <= 0)
                    {
                        Console.WriteLine($"{enemy.Name}을(를) 처치했습니다!");
                        Console.ReadKey();
                        return VIEW_TYPE.DUNGEONCLEAR;
                    }

                    // 몬스터 반격
                    int enemyDamage = enemy.Atk - character.TotalDef;
                    if (enemyDamage < 0) enemyDamage = 0;
                    character.Hp -= enemyDamage;
                    Console.WriteLine($"{enemy.Name}이(가) 플레이어에게 {enemyDamage}의 데미지를 입혔습니다!");

                    if (character.Hp <= 0)
                    {
                        Console.WriteLine("플레이어가 쓰러졌습니다...");
                        Console.ReadKey();
                        return VIEW_TYPE.DUNGEONCLEAR;
                    }

                    Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                    return VIEW_TYPE.BATTLE;

                case 2:
                    Console.WriteLine("플레이어가 도망쳤습니다...");
                    Console.ReadKey();
                    return VIEW_TYPE.MAIN;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return VIEW_TYPE.BATTLE;
            }
        }
    }


    public class MonsterViewer : Viewer
    {
        protected Monster currentMonster;

        public MonsterViewer(Monster monster)
        {
            this.currentMonster = monster;
            StartIndex = 1;
            EndIndex = 2;
        }

        public override void ViewAction()
        {
            Console.Clear();
            Console.WriteLine(" 몬스터 정보");
            Console.WriteLine("====================");

            if (currentMonster == null)
            {
                Console.WriteLine("몬스터 정보가 없습니다.");
            }
            else
            {
                Console.WriteLine($"이름: {currentMonster.Name}");
                Console.WriteLine($"레벨: {currentMonster.Level}");
                Console.WriteLine($"체력: {currentMonster.Hp}/{currentMonster.MaxHp}");
                Console.WriteLine($"공격력: {currentMonster.Atk}");
                Console.WriteLine($"보스 여부: {(currentMonster.Grade == MONSTER_GRADE.BOSS ? " 보스" : " 일반")}");
            }

            Console.WriteLine("====================");
            Console.WriteLine("1. 전투 시작");
            Console.WriteLine("2. 메인으로 돌아가기");

            int input = GetInput();
            VIEW_TYPE nextView = NextView(input);
            SceneManager.Instance.SwitchScene(nextView);
        }

        public override VIEW_TYPE NextView(int input)
        {
            switch (input)
            {
                case 1:
                    GameManager.Instance.BattleEnemy = currentMonster; // 전투 대상 설정
                    return VIEW_TYPE.BATTLE;

                case 2:
                    return VIEW_TYPE.MAIN;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return VIEW_TYPE.MONSTER;
            }
        }
    }
}





