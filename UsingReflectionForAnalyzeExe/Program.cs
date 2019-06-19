using System;
using System.Reflection;


namespace UsingReflectionForAnalyzeExe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int val;

            // Загрузить сборку MyClass.exe
            Assembly asm = Assembly.LoadFrom(
                @"C:\Users\user\source\repos\MyClasses\MyClasses\bin\Debug\MyClasses.exe"
                );

            // Обнаружить типы, содержащиеся в сборке MyClass.exe
            Type[] alltypes = asm.GetTypes();
            foreach (Type temp in alltypes)
                Console.WriteLine("Найдено: " + temp.Name);

            Console.WriteLine();
            

            // Использовать второй тип, в данном случае - класс MyClass.
            Type t = alltypes[1]; // использовать второй найденный класс
            Console.WriteLine("Использовано: " + t.Name);

            // Получить сведения о консрукторе.
            ConstructorInfo[] ci = t.GetConstructors();

            Console.WriteLine("Доступные конструкторы: ");
            foreach (ConstructorInfo c in ci) {
                // Вывести возвращаемый тип и имя.
                Console.Write(" " + t.Name + "(");

                // Вывести параметры.
                ParameterInfo[] pi = c.GetParameters();
                for (int i=0; i < pi.Length; i++) {
                    Console.Write(pi[i].ParameterType.Name + " " + pi[i].Name);
                    if (i + 1 < pi.Length)
                        Console.Write(", ");
                }

                Console.WriteLine(")");
            }

            Console.WriteLine();

            // Найти подходящий конструктор.
            int x;
            for (x = 0; x < ci.Length; x++) {
                ParameterInfo[] pi = ci[x].GetParameters();
                if (pi.Length == 2)
                    break;
            }

            if (x == ci.Length)
            {
                Console.WriteLine("Подходящий конструктор не найден.");
                return;
            }
            else
                Console.WriteLine("Найден конструктор с двумя параметрами.\n");

            // Сконструировать объект.
            object[] consargs = new object[2];
            consargs[0] = 10;
            consargs[1] = 20;
            object reflectOb = ci[x].Invoke(consargs);

            Console.WriteLine("\nВызов методов для объекта reflectOb.\n");
            MethodInfo[] mi = t.GetMethods();

            // Вызвать каждый метод.
            foreach (MethodInfo m in mi) {
                
                // Получить параметры.
                ParameterInfo[] pi = m.GetParameters();

                if (m.Name.CompareTo("Set") == 0 && pi[0].ParameterType == typeof(int))
                {
                    object[] _args = new object[2];
                    _args[0] = 9;
                    _args[1] = 18;
                    m.Invoke(reflectOb, _args);
                }
                else if (m.Name.CompareTo("Set") == 0 && pi[0].ParameterType == typeof(double))
                {
                    object[] _args = new object[2];
                    _args[0] = 1.12;
                    _args[1] = 23.4;
                    m.Invoke(reflectOb, _args);
                }
                else if (m.Name.CompareTo("Sum") == 0)
                {
                    val = (int)m.Invoke(reflectOb, null);
                    Console.WriteLine($"Сумма равна {val}");
                }
                else if (m.Name.CompareTo("IsBetween") == 0)
                {
                    object[] _args = new object[1];
                    _args[0] = 14;
                    if ((bool)m.Invoke(reflectOb, _args))
                        Console.WriteLine("Значение 14 находится между x и у");
                }
                else if (m.Name.CompareTo("Show") == 0)
                {
                    m.Invoke(reflectOb, null);
                }

            }

            Console.ReadKey();
        }
    }
}
