using System;
using TeamTodayTextRPG;

namespace TeamTodayTextRPG
{
    public class SceneManager
    {
        public Viewer CurrentViewer { get; set; }

        private static readonly Lazy<SceneManager> lazyInstance = new Lazy<SceneManager>(() => new SceneManager());
        public static SceneManager Instance => lazyInstance.Value;

        public SceneManager()
        {
            //『효빈』 초기 생성시 메인뷰어로 들어가도록 하겠습니다.
            //         추후에 가람님의 '그것'을 초기화면으로 설정하도록 바꿀예정입니다. 예를들어...IntroViewer 처럼요
            CurrentViewer = new MainViewer();
        }

        public void SwitchScene(VIEW_TYPE viewType)
        {
            // 새로운 뷰어 할당
            switch (viewType)
            {
                case VIEW_TYPE.MAIN:
                    CurrentViewer = new MainViewer();
                    break;
                case VIEW_TYPE.STATUS:
                    CurrentViewer = new StatusViewer();
                    break;
                case VIEW_TYPE.INVENTORY:
                    CurrentViewer = new InventoryViewer();
                    break;
                case VIEW_TYPE.EQUIP:
                    CurrentViewer = new EquipViewer();
                    break;
                case VIEW_TYPE.SHOP:
                    CurrentViewer = new ShopViewer();
                    break;
                case VIEW_TYPE.PURCHASE:
                    CurrentViewer = new PurchaseViewer();
                    break;
                case VIEW_TYPE.SALE:
                    CurrentViewer = new SaleViewer();
                    break;

                case VIEW_TYPE.DUNGEON:
                    CurrentViewer = new DungeonViewer();
                    break;
                case VIEW_TYPE.DUNGEONCLEAR:
                    CurrentViewer = new DungeonClearViewer();
                    break;
                case VIEW_TYPE.REST:
                    CurrentViewer = new RestViewer();
                    break;

                case VIEW_TYPE.BATTLE:
                    CurrentViewer = new BattleViewer();
                    break;
                    //case VIEW_TYPE.MONSTER:
                    // GameManager에서 직접적으로 몬스터 객체를 가져오는 방식으로 수정
                    /* 『효빈』GameManager에서 Dungeon을 관리하게 하고 
                               1) 던전에 입장시에 몬스터들을 관리하는 List<Monster>를 생성  << 데이터 방식은 던전 설계에 따라 바뀔수도 있을 것 같아요
                               2) 랜덤 값을 이용하여 랜덤하게 몬스터 리스트를 초기화
                               3) 해당 정보는 GameManager.Instance.Dungeon.MonsterList  << 이런식으로 정보를 받아와서 사용하면 될것 같습니다.
                     */
                    // 『효빈』하단의 코드처럼 매개변수로 굳이 받아오지 않아도 된다는 뜻입니다! :)
                    // Monster currentMonster = gameManager.CurrentMonster;//*GameManager에서 속성을 추가 후 받아야함
                    //currentViewer = new MonsterViewer(currentMonster);

                    //CurrentViewer = new MonsterViewer();
                    // break;
            }
            ShowCurrentView();
        }

        //『효빈』선택지 입력 시 다음 화면으로의 전환
        public VIEW_TYPE ChangeNextView()
        {
            return CurrentViewer.NextView(InputAction(CurrentViewer.StartIndex, CurrentViewer.EndIndex));
        }

        // 새로운 뷰어의 화면 출력
        public void ShowCurrentView()
        {
            if (CurrentViewer != null)
            {
                CurrentViewer.ViewAction();  // gameManager 객체를 넘기기
            }
        }

        // 『효빈』초기 캐릭터 설정 (플레이어 이름, 플레이어할 캐릭터의 직업)을 도와주는 함수 입니다.
        public void Intro()
        {
            string? name = string.Empty;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.ResetColor();

            while (name == string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("원하시는 이름을 설정해주세요.");
                Console.ResetColor();
                Console.Write("\n입력 >> ");
                name = Console.ReadLine();
                if (name == string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("이름을 제대로 입력해주세요.\n");
                    Console.ResetColor();
                }
                else
                {
                    bool check = true;

                    while (check)
                    {
                        int num = 0;
                        Console.Write("입력하신 이름은 『 ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(name);
                        Console.ResetColor();
                        Console.WriteLine(" 』입니다.\n");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("1. 저장");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("2. 취소\n");
                        Console.ResetColor();

                        num = SceneManager.Instance.InputAction(1, 2);

                        if (num == 1) check = false;
                        else if (num == 2)
                        {
                            check = false;
                            name = string.Empty;
                        }
                    }
                }
            }
            Console.Clear();

            int classCode = 0;
            while (classCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("원하시는 직업을 설정해주세요.\n");
                Console.ResetColor();
                Console.WriteLine("1. 전사\n2. 마법사\n3. 도적\n");

                classCode = SceneManager.Instance.InputAction(1, 3);
                GameManager.Instance.Player.SetCharacter(classCode, name);
            }
            Console.Clear();
        }

        /* 『효빈』
            꾸준히 호출될 선택지 입력합수입니다.
            매개변수로 선택지의 첫번째 번호와 마지막 번호를 받습니다. 
            ex) 1.아이템구매 2. 아이템판매 0.나가기 
                ...이라면 startIndex = 0, endIndex = 2
            리턴 값으로는 "고른 선택지의 번호"를 반환합니다.
        */
        public int InputAction(int startIndex, int endIndex)
        {
            string rtnStr = string.Empty;
            int num = -1;
            bool check = false;
            while (!check)
            {
                Console.Write("원하시는 행동을 입력해주세요.\n>>");
                rtnStr = Console.ReadLine();
                if (rtnStr == string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("아무 행동도 입력하지 않으셨습니다.\n");
                    Console.ResetColor();
                }
                else
                {
                    if (int.TryParse(rtnStr, out num))
                    {
                        if (num < startIndex && num > endIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("선택지 내에서 입력해주세요.\n");
                            Console.ResetColor();
                        }
                        else check = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("숫자만 입력해주세요.\n");
                        Console.ResetColor();
                    }
                }
            }
            return num;
        }

        /*
        public void ShowName(string name)
        {
            Console.Write(name + "\t| ");
        }
        public void ShowAtk(int atk)
        {
            if (atk > 0) Console.Write("공격력 +" + atk + "\t| ");
            else Console.Write("공격력 -" + atk + "\t| ");
        }
        public void ShowDef(int def)
        {
            if (def > 0) Console.Write("방어력 +" + def + "\t| ");
            else Console.Write("방어력 -" + def + "\t| ");
        }

        public void ShowInventory(VIEW_TYPE view)
        {
            if (GameManager.Instance.Player.Bag != null)
            {
                int count = 0;
                foreach (var item in GameManager.Instance.Player.Bag)
                {
                    if (view == VIEW_TYPE.EQUIP) Console.Write($"- {++count} ");
                    else Console.Write("- ");

                    if (gm.Player.CheckEquip(item))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("[E]");
                        Console.ResetColor();
                    }

                    ShowName(DataManager.Instance.ItemDB.List[item].Name);
                    if (DataManager.Instance.ItemDB.List[item].Atk != 0) ShowAtk(DataManager.Instance.ItemDB.List[item].Atk);
                    if (DataManager.Instance.ItemDB.List[item].Def != 0) ShowDef(DataManager.Instance.ItemDB.List[item].Def);
                    Console.WriteLine(DataManager.Instance.ItemDB.List[item].Text);
                }

            }
        }

        public void ShowShop(VIEW_TYPE view)
        {
            if (DataManager.Instance.ItemDB.List != null)
            {
                foreach (var item in DataManager.Instance.ItemDB.List)
                {
                    if (GameManager.Instance.Player.CheckBag(item.Code)) Console.ForegroundColor = ConsoleColor.DarkGray;

                    if (view == VIEW_TYPE.PURCHASE) Console.Write("- " + (item.Code + 1) + " " + item.Name + "\t| ");
                    else Console.Write("- " + item.Name + "\t| ");

                    if (item.Atk != 0) ShowAtk(item.Atk);
                    if (item.Def != 0) ShowDef(item.Def);
                    Console.Write(item.Text + "\t| ");

                    if (GameManager.Instance.Player.CheckBag(item.Code))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("구매 완료");
                        Console.ResetColor();
                    }
                    else Console.WriteLine(item.Value + " G");
                }
            }
        }

        public void ShowShopSale()
        {
            int count = 0;
            if (GameManager.Instance.Player.Bag != null)
            {
                foreach (var item in GameManager.Instance.Player.Bag)
                {
                    Console.Write("- " + (++count) + " " + DataManager.Instance.ItemDB.List[item].Name + "\t| ");
                    if (DataManager.Instance.ItemDB.List[item].Atk != 0) ShowAtk(DataManager.Instance.ItemDB.List[item].Atk);
                    if (DataManager.Instance.ItemDB.List[item].Def != 0) ShowDef(DataManager.Instance.ItemDB.List[item].Def);
                    Console.Write(DataManager.Instance.ItemDB.List[item].Text + "\t| ");
                    Console.WriteLine((int)(DataManager.Instance.ItemDB.List[item].Value * 0.85) + " G");
                }
            }
        }*/
    }
}
