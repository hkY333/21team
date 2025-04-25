using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Numerics;
using System.Diagnostics;

namespace TeamTodayTextRPG
{
    public enum ITEM_TYPE
    {
        WEAPON = 0,        //무기
        ARMOR = 1,         //방어구
        CONSUMABLE = 2,    //소모품
    }

    public class Item
    {
        public int Code { get; private set; }
        public string Name { get; private set; }
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public int Hp { get; private set; }
        public int Mp { get; private set; }
        public string Text { get; private set; }
        public int Value { get; private set; }
        public ITEM_TYPE Type { get; set; }

        public Item(string[] str) //Parse로 데이터 변환
        {
            Code = int.Parse(str[0]);
            Name = str[1];
            Atk = int.Parse(str[2]);
            Def = int.Parse(str[3]);
            Hp = int.Parse(str[4]);
            Mp = int.Parse(str[5]);
            Text = str[6];
            Value = int.Parse(str[7]);
            if (int.Parse(str[8]) == (int)ITEM_TYPE.WEAPON)
            {
                Type = ITEM_TYPE.WEAPON;
            }
            else if (int.Parse(str[8]) == (int)ITEM_TYPE.ARMOR)
            {
                Type = ITEM_TYPE.ARMOR;
            }
            else if (int.Parse(str[8]) == (int)ITEM_TYPE.CONSUMABLE)
            {
                Type = ITEM_TYPE.CONSUMABLE;
            }


            // 직업 제한 파싱
            List<CHAR_TYPE> allowedJobs = new List<CHAR_TYPE>();

            if (Text.Contains("전사"))
            {
                allowedJobs.Add(CHAR_TYPE.WARRIOR);
            }
            else if (Text.Contains("도적"))
            {
                allowedJobs.Add(CHAR_TYPE.ASSASSIN);
            }
            else if (Text.Contains("마법사"))
            {
                allowedJobs.Add(CHAR_TYPE.MAGICIAN);
            }


            //items.Add(new Item(Code, Name, Atk, Def, Hp, Mp, Text, Value, Type, allowedJobs));

        }
    }
    /*
    public class HpPotion : Item
    {
        public int HealAmount { get; private set; } = 20; //항상 20을 회복

        public HpPotion(int code, string name, string text, int value, int healAmount)
            : base(code, name, text, value, ITEM_TYPE.CONSUMABLE)
        {
            // HealAmount는 이미 20으로 고정됨
        }

        public override void Use(Player player)
        {
            player.Heal(HealAmount);
            Console.WriteLine($" HP포션을 사용하여 체력을 {HealAmount} 회복했습니다.");
        }
    }
    public class MpPotion : Item
    {
        public int ManaAmount { get; private set; } = 20; // 항상 20을 회복

        public MpPotion(int code, string name, string text, int value, int manaAmount)
            : base(code, name, text, value, ITEM_TYPE.CONSUMABLE)
        {
            // ManaAmount는 20으로 고정
        }

        public override void Use(Player player)
        {
            player.RecoverMana(ManaAmount);
            Console.WriteLine($" MP포션을 사용하여 마나를 {ManaAmount} 회복했습니다.");
        }
    }*/
}
