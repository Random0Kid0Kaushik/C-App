using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MushroomPocket
{
    class Program
    {
        static void Main(string[] args)
        {
            //MushroomMaster criteria list for checking character transformation availability. 
            /*************************************************************************
             * PLEASE DO NOT CHANGE THE CODES FROM LINE 15-19
             *************************************************************************/
            List<MushroomMaster> mushroomMasters = new List<MushroomMaster>()
            {
                new MushroomMaster("Daisy", 2, "Peach"),
                new MushroomMaster("Wario", 3, "Mario"),
                new MushroomMaster("Waluigi", 1, "Luigi"),
                new MushroomMaster("Abbas", 1, "Shawarma Man"),
                new MushroomMaster("Zygarde", 1, "Complete Forme"),
                new MushroomMaster("Gojo", 1, "Infinity")
            };

            //Use "Environment.Exit(0);" if you want to implement an exit of the console program

            List<Character> MyDbContext = new List<Character>();

            Console.Title = "Mushroom Pocket";
            while (true)
            {
                Console.WriteLine("**************************************************");
                Console.WriteLine("Welcome to Mushroom Pocket App");
                Console.WriteLine("**************************************************");
                Console.WriteLine("Please only enter [1,2,3,4,5,6,7,8,9,10,11] or Q to quit:");

                string[] options =
                {
                    "Add Mushroom's character to my pocket",
                    "List character(s) in my Pocket",
                    "Check if I can transform my characters",
                    "Transform character(s)",
                    "Gain Exp for all characters in my pocket",
                    "View Details of Mushroom character",
                    "Get Background Information on a Character",
                    "Get Information on Character Skill",
                    "Create a Custom Character",
                    "View strength and weakness of the different types ",
                    "How to Play",
                };

                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"({i + 1}). {options[i]}");
                }

                string option = Console.ReadLine();
                if (option != null)
                {
                    switch (option)
                    {
                        case "Q":
                        case "q":
                            Environment.Exit(0);
                            break;
                        case "1":
                            AddMushroomCharacter();
                            break;
                        case "2":
                            ListCharacters();
                            break;
                        case "3":
                            CheckTransformCharacter();
                            break;
                        case "4":
                            TransformCharacter();
                            break;
                        case "5":
                            GainExp();
                            break;
                        case "6":
                            CharacterDetails();
                            break;
                        case "7":
                            GetCharacterBackground();
                            break;
                        case "8":
                            GetCharacterSkill();
                            break;
                        case "9":
                            CreateCustomCharacter();
                            break;
                        case "10":
                            ViewTypeChart();
                            break;
                        case "11":
                            HowToPlay();
                            break;
                        default:
                            Console.WriteLine("Invalid option, please enter a valid option");
                            break;
                    }
                }
            }

            // Functions
            void HowToPlay()
            {
                Console.WriteLine("Welcome to Mushroom Pocket!");
                Console.WriteLine("In this game, you can collect and train various characters.");
                Console.WriteLine("Here are the basic instructions on how to play:");

                Console.WriteLine("1. Gain Experience (Exp):");
                Console.WriteLine("   - Use the 'GainExp' function to gain experience for all characters in your pocket.");
                Console.WriteLine("   - Exp is important for character growth and evolution.\n");

                Console.WriteLine("2. Add Mushroom Character:");
                Console.WriteLine("   - Use the 'AddMushroomCharacter' function to add a mushroom character to your pocket.");
                Console.WriteLine("   - Enter the character name when prompted (waluigi, wario, daisy, abbas, zygarde, gojo).\n");

                Console.WriteLine("3. List Characters:");
                Console.WriteLine("   - Use the 'ListCharacters' function to list all characters in your pocket.\n");

                Console.WriteLine("4. Check Transform Character:");
                Console.WriteLine("   - Use the 'CheckTransformCharacter' function to check if your characters can transform.");
                Console.WriteLine("   - The function will display the transformation details for each character.\n");

                Console.WriteLine("5. Transform Character:");
                Console.WriteLine("   - Use the 'TransformCharacter' function to transform a specific character in your pocket.");
                Console.WriteLine("   - Enter the character name when prompted.\n");

                Console.WriteLine("6. Gain Exp:");
                Console.WriteLine("   - Use the 'GainExp' function to gain experience for all characters in your pocket.\n");

                Console.WriteLine("7. Character Details:");
                Console.WriteLine("   - Use the 'CharacterDetails' function to view the details of a specific character.");
                Console.WriteLine("   - Enter the character name when prompted.\n");

                Console.WriteLine("8. Get Character Background:");
                Console.WriteLine("   - Use the 'GetCharacterBackground' function to get the background information of a specific character.");
                Console.WriteLine("   - Enter the character name when prompted.\n");

                Console.WriteLine("9. View Character Skill:");
                Console.WriteLine("   - Use the 'GetCharacterSkill' function to get the skill information of a specific character.");
                Console.WriteLine("   - Enter the character name when prompted.\n");

                Console.WriteLine("10. Create Custom Character:");
                Console.WriteLine("   - Use the 'CreateCustomCharacter' function to create a custom character with your desired attributes.");
                Console.WriteLine("   - Follow the prompts to enter the character name, HP, attack, defense, special attack, special defense, speed, type 1, type 2, skill, and transform to.\n");

                Console.WriteLine("11. View strengths and weaknesses of the types:");
                Console.WriteLine("   - Use the 'ViewTypeChart' function to view the effectiveness chart of different character types.");
            }
            void AddMushroomCharacter()
            {
                // Handle adding mushroom character to the pocket
                Console.Write("Enter the character name: ");
                string characterName = Console.ReadLine();

                // Early return for invalid character
                if (!TryCreateCharacter(characterName, out Character character))
                {
                    Console.WriteLine("Invalid character name");
                    return;
                }

                // Get custom HP and EXP values from the user
                Console.Write("Enter the character's HP: ");
                if (!int.TryParse(Console.ReadLine(), out int hp))
                {
                    Console.WriteLine("Invalid HP input. Input an integer.");
                    return;
                }
                character.Hp = hp;

                Console.Write("Enter the character's EXP: ");
                if (!int.TryParse(Console.ReadLine(), out int exp))
                {
                    Console.WriteLine("Invalid EXP input. Defaulting to character's base EXP.");
                    exp = 0; // Use the default EXP if input is invalid
                }
                character.Exp = exp;

                // Add the character to the database using Entity Framework
                Character characterToAdd = new Character
                {
                    CharacterName = character.CharacterName,
                    Hp = character.Hp,
                    Atk = character.Atk,
                    Def = character.Def,
                    SpAtk = character.SpAtk,
                    SpDef = character.SpDef,
                    Speed = character.Speed,
                    Exp = character.Exp,
                    Type1 = character.Type1,
                    Type2 = character.Type2,
                    Skill = character.Skill,
                    TransformTo = character.TransformTo
                };

                MyDbContext.Add(characterToAdd);

                Console.WriteLine($"{characterName} has been added to the pocket");
            }

            bool TryCreateCharacter(string characterName, out Character character)
            {
                character = null;

                switch (characterName.ToLower()) // Case insensitive name check
                {
                    case "abbas":
                        character = new Abbas();
                        return true;
                    case "waluigi":
                        character = new Waluigi();
                        return true;
                    case "wario":
                        character = new Wario();
                        return true;
                    case "daisy":
                        character = new Daisy();
                        return true;
                    case "zygarde":
                        character = new Zygarde();
                        return true;
                    case "gojo":
                        character = new Gojo();
                        return true;
                    default:
                        return false;
                }
            }

            void ListCharacters()
            {
                // Get all characters in the pocket
                Character[] characters = MyDbContext.ToArray();

                if (characters.Length == 0)
                {
                    Console.WriteLine("No characters in the pocket");
                }
                else
                {
                    foreach (Character character in characters)
                    {
                        Console.WriteLine("--------------------");
                        Console.WriteLine($"Character Name: {character.CharacterName}");
                        Console.WriteLine($"HP: {character.Hp}");
                        Console.WriteLine($"Atk: {character.Atk}");
                        Console.WriteLine($"Def: {character.Def}");
                        Console.WriteLine($"SpAtk: {character.SpAtk}");
                        Console.WriteLine($"SpDef: {character.SpDef}");
                        Console.WriteLine($"Speed: {character.Speed}");
                        Console.WriteLine($"Exp: {character.Exp}");
                        Console.WriteLine($"Type 1: {character.Type1}");
                        Console.WriteLine($"Type 2: {character.Type2}");
                        Console.WriteLine($"Skill: {character.Skill}");
                        Console.WriteLine($"Transform To: {character.TransformTo}");
                        Console.WriteLine("--------------------");
                    }
                }
            }

            void CheckTransformCharacter()
            {
                // Get all characters in the pocket
                Character[] characters = MyDbContext.ToArray();

                if (characters.Length == 0)
                {
                    Console.WriteLine("No characters in the pocket");
                }
                else
                {
                    foreach (Character character in characters)
                    {
                        // Check if the character can transform based on the mushroom masters
                        MushroomMaster master = mushroomMasters.FirstOrDefault(m => m.Name == character.CharacterName);
                        if (master != null)
                        {
                            Console.WriteLine($"{character.CharacterName} --> {master.TransformedTo}");
                        }
                        else
                        {
                            Console.WriteLine($"{character.CharacterName} --> Cannot Transform");
                        }
                    }
                }
            }

            void TransformCharacter()
            {
                Console.Write("Enter the character name to transform: ");
                string characterName = Console.ReadLine();

                Character character = MyDbContext.FirstOrDefault(c => c.CharacterName == characterName);
                if (character == null)
                {
                    Console.WriteLine($"Character '{characterName}' not found in the pocket.");
                    return;
                }

                MushroomMaster master = mushroomMasters.FirstOrDefault(m => m.Name == character.CharacterName);
                if (master != null)
                {
                    // Reset experience before transformation
                    character.Exp = 0;

                    character.CharacterName = master.TransformedTo;
                    Console.WriteLine($"{characterName} has transformed to {master.TransformedTo}");
                }
                else
                {
                    Console.WriteLine($"{characterName} cannot transform.");
                }
            }

            void GainExp()
            {
                // Implement the logic to gain experience for the characters in the pocket
                foreach (Character character in MyDbContext)
                {
                    character.GainExp();
                }

                Console.WriteLine("Exp has been gained for all characters in the pocket");
            }

            void CharacterDetails()
            {
                Console.Write("Enter the character name: ");
                string characterName = Console.ReadLine();

                // Use 'using' for automatic disposal

                // Find character by name using FirstOrDefault
                Character character = MyDbContext.FirstOrDefault(c => c.CharacterName == characterName);

                if (character == null)
                {
                    Console.WriteLine($"Character '{characterName}' not found in the pocket.");
                    return;
                }

                Console.WriteLine("--------------------");
                Console.WriteLine($"Character Name: {character.CharacterName}");
                Console.WriteLine($"HP: {character.Hp}");
                Console.WriteLine($"Atk: {character.Atk}");
                Console.WriteLine($"Def: {character.Def}");
                Console.WriteLine($"SpAtk: {character.SpAtk}");
                Console.WriteLine($"SpDef: {character.SpDef}");
                Console.WriteLine($"Speed: {character.Speed}");
                Console.WriteLine($"Exp: {character.Exp}");
                Console.WriteLine($"Type 1: {character.Type1}");
                Console.WriteLine($"Type 2: {character.Type2}");
                Console.WriteLine($"Skill: {character.Skill}");
                Console.WriteLine($"Transform To: {character.TransformTo}");
                Console.WriteLine("--------------------");

            }

            void GetCharacterBackground()
            {
                Console.Write("Enter the character name: ");
                string characterName = Console.ReadLine();

                Character character = MyDbContext.Find(c => c.CharacterName == characterName);

                if (character == null)
                {
                    Console.WriteLine($"Character '{characterName}' not found in the pocket.");
                    return;
                }

                // Call a function or access a property to get the character's background information
                string characterBackground = GetCharacterBackgroundInfo(character);
                // Replace with your actual background retrieval logic

                Console.WriteLine("--------------------");
                Console.WriteLine($"Character Name: {character.CharacterName}");
                Console.WriteLine($"Background: {characterBackground}");
                Console.WriteLine("--------------------");
            }

            string GetCharacterBackgroundInfo(Character character)
            {
                switch (character.CharacterName.ToLower())
                {
                    case "waluigi":
                        return "Waluigi: Unlike Wario who's the antagonist version of Mario, Waluigi's backstory is a bit less clear. He was created specifically as Wario's partner for Mario Tennis. There are theories suggesting him to be Luigi's rival due to their contrasting designs and personalities. However, his official connection to Luigi remains unconfirmed. Waluigi is known for his mischievous and competitive spirit, often causing trouble alongside Wario in various sports and party games.";
                    case "luigi":
                        return "Luigi: Mario's younger brother, Luigi is known for his unwavering loyalty and support for his brother. Though often portrayed as cowardly, Luigi demonstrates bravery when facing challenges, especially when it comes to rescuing Princess Peach. Throughout the games, Luigi has had various solo adventures showcasing his own unique skills and overcoming his fears.";
                    case "wario":
                        return "Wario: This greedy and anti-hero character is Mario's rival. Unlike Mario's heroic plumber persona, Wario prefers making money through somewhat dubious means. He often creates wacky inventions or runs his own twisted version of businesses like restaurants and microgames. Despite their rivalry, Wario and Mario occasionally team up against bigger threats.";
                    case "mario":
                        return "Mario is the hero of the Mushroom Kingdom and is known for his bravery and jumping skills.";
                    case "daisy":
                        return "Daisy is a princess from the Sarasaland kingdom and a close friend of Peach.";
                    case "peach":
                        return "Princess Peach: The benevolent ruler of the Mushroom Kingdom, Princess Peach is kind and compassionate. Unfortunately, her kindness makes her a frequent target for the villainous Bowser. Despite being kidnapped often, Peach is a resourceful and courageous character who can hold her own in various spin-off games, showcasing her athleticism and fighting skills.";
                    // Add more cases for other characters
                    case "abbas":
                        return "Abbas is a character who can transform into a powerful Shawarma Man.";
                    case "zygarde":
                        return "This Legendary Pokémon from the Pokémon series is known as the \'Order Pokémon\'. Zygarde has multiple forms and plays a crucial role in maintaining the Kalos region's ecosystem, keeping balance between creation and destruction.";
                    case "gojo":
                        return "Gojo Satoru: From the popular Jujutsu Kaisen series, Gojo is a powerful Jujutsu sorcerer who serves as a teacher and mentor.  He possesses immense strength and abilities, making him a formidable character.";
                    default:
                        return "Background information not available.";
                }
            }

            //Get Character Skill
            void GetCharacterSkill()
            {
                Console.Write("Enter the character name: ");
                string characterName = Console.ReadLine();

                Character character = MyDbContext.Find(c => c.CharacterName == characterName);

                if (character == null)
                {
                    Console.WriteLine($"Character '{characterName}' not found in the pocket.");
                    return;
                }

                // Call a function or access a property to get the character's skill information
                string characterSkill = GetCharacterSkillInfo(character);
                // Replace with your actual skill retrieval logic

                Console.WriteLine("--------------------");
                Console.WriteLine($"Character Name: {character.CharacterName}");
                Console.WriteLine($"Skill: {characterSkill}");
                Console.WriteLine("--------------------");
            }

            string GetCharacterSkillInfo(Character character)
            {
                switch (character.CharacterName.ToLower())
                {
                    case "waluigi":
                        return "Skill: Agility - Waluigi is known for his agility and acrobatic skills, allowing him to perform impressive moves on the tennis court and other sports arenas.";
                    case "luigi":
                        return "Skill: Precision and Accuracy - Luigi is known for his precision and accuracy, especially in games like Luigi's Mansion where he captures ghosts with his trusty Poltergust.";
                    case "wario":
                        return "Skill: Strength - Wario's brute strength is his key skill, allowing him to break through obstacles and overpower opponents in various games.";
                    case "mario":
                        return "Skill: Combat Skills - Mario is a skilled fighter with expertise in jumping, fireballs, and power-ups that help him defeat enemies and save the Mushroom Kingdom.";
                    case "daisy":
                        return "Skill: Leadership - Daisy's leadership skills shine in sports games, where she motivates her team to victory and showcases her competitive spirit.";
                    case "peach":
                        return "Skill: Grace - Princess Peach's grace and elegance are her defining traits, whether she's racing in karts or playing tennis. She combines style with skill to outshine her opponents.";
                    // Add more cases for other characters
                    case "abbas":
                        return "Skill: Way of the Shawarma - Abbas uses his Shawarma skills to heal and buff his allies, making him a valuable support character in battles.";
                    case "zygarde":
                        return "Skill: Core Enforcer - Zygarde's signature move, Core Enforcer, unleashes a powerful attack that can devastate opponents and turn the tide of battle.";
                    case "gojo":
                        return "Skill: Limitless - Gojo's Limitless technique allows him to manipulate space and time, giving him incredible speed and power in combat.";
                    default:
                        return "Skill information not available.";
                }
            }

            void CreateCustomCharacter()
            {
                Console.Write("Enter the character name: ");
                string characterName = Console.ReadLine();

                Console.Write("Enter HP: ");
                if (!int.TryParse(Console.ReadLine(), out int hp))
                {
                    Console.WriteLine("Invalid HP value.");
                    return;
                }

                Console.Write("Enter Attack: ");
                if (!int.TryParse(Console.ReadLine(), out int atk))
                {
                    Console.WriteLine("Invalid Attack value.");
                    return;
                }

                Console.Write("Enter Defense: ");
                if (!int.TryParse(Console.ReadLine(), out int def))
                {
                    Console.WriteLine("Invalid Defense value.");
                    return;
                }

                Console.Write("Enter Special Attack: ");
                if (!int.TryParse(Console.ReadLine(), out int spAtk))
                {
                    Console.WriteLine("Invalid Special Attack value.");
                    return;
                }

                Console.Write("Enter Special Defense: ");
                if (!int.TryParse(Console.ReadLine(), out int spDef))
                {
                    Console.WriteLine("Invalid Special Defense value.");
                    return;
                }

                Console.Write("Enter Speed: ");
                if (!int.TryParse(Console.ReadLine(), out int speed))
                {
                    Console.WriteLine("Invalid Speed value.");
                    return;
                }

                Console.Write("Enter Type 1: ");
                string Type1 = Console.ReadLine();

                Console.Write("Enter Type 2: ");
                string Type2 = Console.ReadLine();

                Console.Write("Enter Skill: ");
                string skill = Console.ReadLine();

                Console.Write("Enter Transform To: ");
                string transformTo = Console.ReadLine();

                Character customCharacter = new CustomCharacter(characterName, hp, atk, def, spAtk, spDef, speed, Type1, Type2, skill, transformTo);
                MyDbContext.Add(customCharacter);
                Console.WriteLine($"Custom character '{characterName}' created.");
            }
        }

        // Character class definition
        public class Character
        {
            public string CharacterName { get; set; }
            public int Hp { get; set; }
            public int Atk { get; set; }
            public int Def { get; set; }
            public int SpAtk { get; set; }
            public int SpDef { get; set; }
            public int Speed { get; set; }
            public int Exp { get; set; }
            public string Type1 { get; set; }
            public string Type2 { get; set; }
            public string Skill { get; set; }
            public string TransformTo { get; set; }

            public virtual void GainExp()
            {
                // Implement the logic for gaining Exp for the character
                Exp += 10; // Example - add 10 Exp
            }

        }

        public class Abbas : Character
        {
            public Abbas()
            {
                CharacterName = "Abbas";
                Hp = 80;
                Atk = 80;
                Def = 135;
                SpAtk = 100;
                SpDef = 115;
                Speed = 50;
                Exp = 0;
                Type1 = "Grass";
                Type2 = "Fighting";
                Skill = "Way of the Shawarma";
                TransformTo = "Shawarma Man";
            }

        }
        public class ShawarmaMan : Character
        {
            public ShawarmaMan()
            {
                CharacterName = "Shawarma Man";
                Hp = 100;
                Atk = 90;
                Def = 140;
                SpAtk = 110;
                SpDef = 125;
                Speed = 40;
                Exp = 0;
                Type1 = "Grass";
                Type2 = "Fighting";
                Skill = "Shawarma Strike";
                TransformTo = "Abbas";
            }

        }

        public class Waluigi : Character
        {
            public Waluigi()
            {
                CharacterName = "Waluigi";
                Hp = 80;
                Atk = 120;
                Def = 80;
                SpAtk = 120;
                SpDef = 80;
                Speed = 120;
                Exp = 0;
                Type1 = "Dark";
                Type2 = "Steel";
                Skill = "Agility";
                TransformTo = "Luigi";
            }

        }

        public class Luigi : Character
        {
            public Luigi()
            {
                CharacterName = "Luigi";
                Hp = 90;
                Exp = 0;
                Atk = 130;
                Def = 80;
                SpAtk = 130;
                SpDef = 100;
                Speed = 130;
                Type1 = "Normal";
                Type2 = "Ghost";
                Skill = "Precision and Accuracy";
                TransformTo = "Waluigi";
            }

        }

        public class Wario : Character
        {
            public Wario()
            {
                CharacterName = "Wario";
                Hp = 80;
                Atk = 80;
                Def = 120;
                SpAtk = 80;
                SpDef = 120;
                Speed = 90;
                Exp = 0;
                Type1 = "Dark";
                Type2 = "Fighting";
                Skill = "Strength";
                TransformTo = "Mario";
            }

        }

        public class Mario : Character
        {
            public Mario()
            {
                CharacterName = "Mario";
                Hp = 90;
                Exp = 0;
                Atk = 110;
                Def = 100;
                SpAtk = 110;
                SpDef = 110;
                Speed = 100;
                Type1 = "Fire";
                Type2 = "Fighting";
                Skill = "Combat Skills";
                TransformTo = "Wario";
            }

        }

        public class Daisy : Character
        {
            public Daisy()
            {
                CharacterName = "Daisy";
                Hp = 80;
                Exp = 0;
                Atk = 100;
                Def = 90;
                SpAtk = 100;
                SpDef = 90;
                Speed = 110;
                Type1 = "Grass";
                Type2 = "Fairy";
                Skill = "Leadership";
                TransformTo = "Peach";
            }

        }

        public class Peach : Character
        {
            public Peach()
            {
                CharacterName = "Peach";
                Hp = 100;
                Atk = 110;
                Def = 80;
                SpAtk = 110;
                SpDef = 80;
                Speed = 120;
                Exp = 0;
                Type1 = "Fairy";
                Type2 = "Psychic";
                Skill = "Grace";
                TransformTo = "Daisy";
            }

        }
        public class Zygarde : Character
        {
            public Zygarde()
            {
                CharacterName = "Zygarde";
                Hp = 100;
                Exp = 0;
                Atk = 125;
                Def = 130;
                SpAtk = 120;
                SpDef = 125;
                Speed = 85;
                Type1 = "Dragon";
                Type2 = "Ground";
                Skill = "Core Enforcer";
                TransformTo = "Complete Forme";
            }

        }

        public class CompleteForme : Character
        {
            public CompleteForme()
            {
                CharacterName = "Complete Forme";
                Hp = 216;
                Atk = 130;
                Def = 145;
                SpAtk = 130;
                SpDef = 145;
                Speed = 60;
                Exp = 0;
                Type1 = "Dragon";
                Type2 = "Ground";
                Skill = "Power Construct";
                TransformTo = "Zygarde";
            }

        }

        public class Gojo : Character
        {
            public Gojo()
            {
                CharacterName = "Gojo";
                Hp = 100;
                Atk = 100;
                Def = 100;
                SpAtk = 100;
                SpDef = 100;
                Speed = 100;
                Exp = 0;
                Type1 = "Cursed";
                Type2 = "Fairy";
                Skill = "Limitless";
                TransformTo = "Infinity";
            }

        }

        public class Infinity : Character
        {
            public Infinity()
            {
                CharacterName = "Infinity";
                Hp = 120;
                Atk = 120;
                Def = 120;
                SpAtk = 120;
                SpDef = 120;
                Speed = 120;
                Exp = 0;
                Type1 = "Cursed";
                Type2 = "Fairy";
                Skill = "Limitless";
                TransformTo = "Gojo";
            }

        }

        public class CustomCharacter : Character
        {
            public CustomCharacter(string characterName, int hp, int atk, int def, int spAtk, int spDef, int speed, string type1, string type2, string skill, string transformTo)
            {
                CharacterName = characterName;
                Hp = hp;
                Atk = atk;
                Def = def;
                SpAtk = spAtk;
                SpDef = spDef;
                Speed = speed;
                Type1 = type1;
                Type2 = type2;
                Exp = 0;
                Skill = skill;
                TransformTo = transformTo;
            }
        }

        static void ViewTypeChart()
        {
            // Define the types and their effectiveness against each other
            string[] types = { "Normal", "Fire", "Water", "Grass", "Electric", "Ice", "Fighting", "Poison", "Ground", "Flying", "Psychic", "Bug", "Rock", "Ghost", "Dragon", "Dark", "Steel", "Fairy", "Cursed" };
            //Ask User which type they want to check
            Console.WriteLine("Which type would you like to check?");
            switch (Console.ReadLine().ToLower())
            {
                case "normal":
                    Console.WriteLine("Weaknesses: Fighting\n");
                    Console.WriteLine("Immunities: Ghost\n");
                    Console.WriteLine("Resistances: None\n");
                    Console.WriteLine("Resisted by: Rock, Steel and Cursed");
                    return;
                case "fire":
                    Console.WriteLine("Weaknesses: Water, Rock, Ground\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Fire, Grass, Ice, Bug, Steel, Fairy\n");
                    Console.WriteLine("Resisted by: Fire, Water, Rock, Dragon, Cursed");
                    return;
                case "water":
                    Console.WriteLine("Weaknesses: Electric, Grass\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Water, Fire, Ice, Steel\n");
                    Console.WriteLine("Resisted by: Water, Grass, Dragon, Cursed");
                    return;
                case "grass":
                    Console.WriteLine("Weaknesses: Fire, Ice, Poison, Flying, Bug\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Water, Electric, Grass, Ground\n");
                    Console.WriteLine("Resisted by: Fire, Grass, Poison, Flying, Bug, Dragon, Steel, Fairy");
                    return;
                case "electric":
                    Console.WriteLine("Weaknesses: Ground\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Electric, Flying, Steel\n");
                    Console.WriteLine("Resisted by: Electric, Grass, Dragon");
                    return;
                case "ice":
                    Console.WriteLine("Weaknesses: Fire, Fighting, Rock, Steel\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances:Water, Ice\n");
                    Console.WriteLine("Resisted by: Fire, Water, Ice, Steel");
                    return;
                case "fighting":
                    Console.WriteLine("Weaknesses: Flying, Psychic, Fairy\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Bug, Rock, Dark\n");
                    Console.WriteLine("Resisted by: Bug, Rock, Dark, Steel, Cursed");
                    return;
                case "poison":
                    Console.WriteLine("Weaknesses: Ground, Psychic\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Grass, Fairy\n");
                    Console.WriteLine("Resisted by: Poison, Ground, Rock, Ghost, Steel");
                    return;
                case "ground":
                    Console.WriteLine("Weaknesses: Water, Grass, Ice\n");
                    Console.WriteLine("Immunities: Electric\n");
                    Console.WriteLine("Resistances: Poison, Rock, Steel, Fire, Electric\n");
                    Console.WriteLine("Resisted by: Poison, Rock, Steel");
                    return;
                case "flying":
                    Console.WriteLine("Weaknesses: Electric, Ice, Rock\n");
                    Console.WriteLine("Immunities: Ground\n");
                    Console.WriteLine("Resistances: Grass, Fighting, Bug\n");
                    Console.WriteLine("Resisted by: Electric, Rock, Steel");
                    return;
                case "psychic":
                    Console.WriteLine("Weaknesses: Bug, Ghost, Dark\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Fighting, Psychic\n");
                    Console.WriteLine("Resisted by: Psychic, Steel");
                    return;
                case "bug":
                    Console.WriteLine("Weaknesses: Fire, Flying, Rock\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Grass, Fighting, Ground\n");
                    Console.WriteLine("Resisted by: Fire, Fighting, Poison, Flying, Ghost, Steel, Fairy");
                    return;
                case "rock":
                    Console.WriteLine("Weaknesses: Water, Grass, Fighting, Ground, Steel\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Normal, Fire, Poison, Flying\n");
                    Console.WriteLine("Resisted by: Fighting, Ground, Steel");
                    return;
                case "ghost":
                    Console.WriteLine("Weaknesses: Ghost, Dark\n");
                    Console.WriteLine("Immunities: Normal, Fighting\n");
                    Console.WriteLine("Resistances: Poison, Bug\n");
                    Console.WriteLine("Resisted by: Psychic, Ghost");
                    return;
                case "dragon":
                    Console.WriteLine("Weaknesses: Ice, Dragon, Fairy\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Fire, Water, Grass, Electric\n");
                    Console.WriteLine("Resisted by: Ice, Dragon, Fairy, Steel");
                    return;
                case "dark":
                    Console.WriteLine("Weaknesses: Fighting, Bug, Fairy\n");
                    Console.WriteLine("Immunities: Psychic\n");
                    Console.WriteLine("Resistances: Ghost, Dark\n");
                    Console.WriteLine("Resisted by: Fighting, Bug, Fairy");
                    return;
                case "steel":
                    Console.WriteLine("Weaknesses: Fire, Fighting, Ground\n");
                    Console.WriteLine("Immunities: Poison\n");
                    Console.WriteLine("Resistances: Normal, Grass, Ice, Flying, Psychic, Bug, Rock, Dragon, Steel, Fairy\n");
                    Console.WriteLine("Resisted by: Fire, Water, Electric, Steel");
                    return;
                case "fairy":
                    Console.WriteLine("Weaknesses: Poison, Steel\n");
                    Console.WriteLine("Immunities: Dragon\n");
                    Console.WriteLine("Resistances: Fighting, Bug, Dark\n");
                    Console.WriteLine("Resisted by: Poison, Steel");
                    return;
                case "cursed":
                    Console.WriteLine("Weaknesses: None\n");
                    Console.WriteLine("Immunities: None\n");
                    Console.WriteLine("Resistances: Normal, Fire, Water, Grass, Electric, Ice, Fighting, Poison, Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Fairy\n");
                    Console.WriteLine("Resisted by: None");
                    return;
            }
        }
}

// MushroomMaster class
public class MushroomMaster
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string TransformedTo { get; set; }

    public MushroomMaster(string name, int level, string transformedTo)
    {
        Name = name;
        Level = level;
        TransformedTo = transformedTo;
    }
}
}