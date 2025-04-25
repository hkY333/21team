namespace TeamTodayTextRPG
{
    enum DATA_TYPE
    {
        CHARACTER,
        MONSTER,
        ITEM,
        DUNGEON
    }
    /*『효빈』
        데이터 매니저입니다.
        모든 데이터를 이곳에서 관리하고 접근합니다. (추후 set함수는 전부 private로 변경할 예정이에요~)

        ============================================================================================
        List<string[]>
        캐릭터 데이터베이스 접근       DataManager.Instance.CharacterDB.List[캐릭터 코드]

        List<string[]>
        몬스터 데이터베이스 접근       DataManager.Instance.MonsterDB.List[몬스터 코드]

        * 아이템과 던전 데이터 베이스도 추후 string[] 리스트로 변경 될 가능성이 높습니다..!*
        List<Item>
        아이템 데이터베이스 접근       DataManager.Instance.ItemDB.List[아이템 코드]

        List<Dungeon>
        던전   데이터베이스 접근       DataManager.Instance.DungeonDB.List[던전 코드]
        ============================================================================================

        아이템 코드는 각 데이터들의 enum으로 관리합니다.
        제가 담당한 Monster로 예를 들자면....

            enum MONSTER_CODE           <= enumerator 는 보통 전부 대문자로 씁니다..! 소문자도 상관은 없어요:)
            {
                트랄랄랄로_트랄랄라 = 0,
                퉁x9_사후르,
                리릴리_라릴라,
                카푸치노_어쌔시노,
                ...
            }
            
            Q1) 위의 enum을 활용하여 리릴리 라릴라의 데이터를 받아와 리릴리 라릴라의 데이터를 얻고 싶다면?
            A1) DataManager.Instance.MonsterDB.List[(int)MONSTER_CODE.리릴리_라릴라]           <- string[]형 데이터입니다.
                                        0.코드 / 1.레벨 / 2.이름 / 3.공격력 / 4.방어력 / 5.체력 / 6.보상골드 / 7.보상경험치 / 8.텍스트  => 이 값들이 들어있습니다

            Q2) 가져온 데이터에서 공격력 값만 얻고 싶다면?
            A2) DataManager.Instance.MonsterDB.List[(int)MONSTER_CODE.리릴리_라릴라][3]           <- 공격력은 3번 인덱스에 있습니다!
    */


    class DataManager
    {
        private static readonly Lazy<DataManager> lazyInstance = new Lazy<DataManager>(() => new DataManager());

        public static DataManager Instance => lazyInstance.Value;

        private DataManager()
        {
            CharacterDB = new CharacterDatabase();
            MonsterDB = new MonsterDatabase();
            ItemDB = new ItemDatabase();
            DungeonDB = new DungeonDatabase();
        }

        public CharacterDatabase CharacterDB { get; private set; }
        public MonsterDatabase MonsterDB { get; private set; }
        public ItemDatabase ItemDB { get; private set; }
        public DungeonDatabase DungeonDB { get; private set; }
    }

    abstract class Database<T>
    {
        public List<T> List { get; set; } = new List<T>();

        public void Init(string data)
        {
            foreach (string[] str in Parsing(data))
            {
                SetData(str);
            }
        }

        public string[][] Parsing(string data)
        {
            string[] parsingTemp = data.Split('#');
            string[][] returnStr = new string[parsingTemp.Length][];

            for (int i = 0; i < parsingTemp.Length; i++)
            {
                returnStr[i] = parsingTemp[i].Split('/');
            }

            return returnStr;
        }
        protected abstract void SetData(string[] parameter);
    }


    class CharacterDatabase : Database<string[]>
    {

        //0.코드/1.직업이름/2.공격력/3.방어력/4.체력/5.마력/6.회피/7.액티브스킬이름/8.패시브스킬이름
        public string Data { get; private set; } =
            "0/전사/12/5/100/40/1/쾅 내려찍기/강철피부.#" +
            "1/마법사/3/3/50/100/3/썬더봁/마력증강.#" +
            "2/도적/7/1/75/75/5/연격/날쌘 움직임.";



        public CharacterDatabase()
        {
            Init(Data);
        }

        protected override void SetData(string[] parameter)
        {
            List.Add(parameter);
        }
    }


    class MonsterDatabase : Database<string[]>
    {
        // 0.코드 / 1.레벨 / 2.이름 / 3.공격력 / 4.방어력 / 5.체력 / 6.보상골드 / 7.보상경험치 / 8.텍스트
        public string Data { get; private set; } =
            "0/1/매우약한 몬스터/3/0/8/50/10/매우 약한 적입니다.#" +
            "1/2/약한 몬스터/5/1/15/100/25/적당히 약한 적입니다.#" +
            "2/3/평범한 몬스터/8/3/20/300/40/평범한 적입니다.#" +
            "2/4/강한 몬스터/10/5/40/600/60/강력한 적입니다.#" +
            "3/5/매우강한 몬스터/12/10/100/1000/80/매우 강력한 적입니다.";

        public MonsterDatabase()
        {
            Init(Data);
        }
        protected override void SetData(string[] parameter)
        {
            List.Add(parameter);
        }
    }

    class ItemDatabase : Database<string[]>
    {
        //0.코드 /1.이름 /2. ATK /3.DEF /4.HP /5.MP /6.설명 /7.가격 /8.타입
        public string Data { get; private set; } =
            "0/천 옷/0/5/0/0/초보자용 옷입니다./500/1#" +
            "1/체인메일/0/18/60/0/전사 전용 갑옷입니다./3000/1#" +
            "2/사제의 로브/0/8/20/0/마법사 전용 갑옷입니다./3000/1#" +
            "3/흑월/0/13/400/0/도적 전용 갑옷입니다./3000/1#" +
            "4/목검/2/0/0/0/초보자용 검입니다./700/2#" +
            "5/글라디우스/5/0/0/0/전사 전용 무기입니다./4000/0#" +
            "6/위저드 완드/3/0/0/5/마법사 전용 무기입니다./4000/0#" +
            "7/단검/7/0/0/0/도적 전용 무기입니다./4000/0#" +
            "8/HP 포션/0/0/0/HP 20을 채워줍니다./300/2#" +
            "9/MP 포션/0/0/0/MP 20을 채워줍니다./300/2";

        public ItemDatabase()
        {
            Init(Data);
        }
        protected override void SetData(string[] parameter)
        {
            List.Add(parameter);
        }
        /*
        public ITEM_TYPE GetTypetoCode(int code) {
                //return List[code][8]; 
        }*/
    }


    class DungeonDatabase : Database<string[]>
    {
        // 0.코드 / 1.던전 이름 / 2.골드 보상 / 3.경험치 보상 / 4.몬스터 하한 레벨 / 5.몬스터 상한 레벨 / 6.난이도
        public string Data { get; private set; } =
            "0/쉬움 던전/1000/500/1/2/0#" +
            "1/일반 던전/1700/800/2/4/1#" +
            "2/어려움 던전/2500/1500/4/5/2#" +
            "3/헬 던전/10000/3000/5/5/3";

        public DungeonDatabase()
        {
            Init(Data);
        }
        protected override void SetData(string[] parameter)
        {
            List.Add(parameter);
        }
    }
}