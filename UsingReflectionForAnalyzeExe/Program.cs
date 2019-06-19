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

            // Испльзовать первый обнаруженный конструктор.
            ParameterInfo[] cpi = ci[0].GetParameters();
            object reflectOb;

            if (cpi.Length > 0)
            {
                object[] consargs = new object[cpi.Length];

                // Инициализировать аргументы.
                for (int n = 0; n < cpi.Length; n++)
                {
                    consargs[n] = 10 + n * 20;
                }

                // Сконструировать объект.
                reflectOb = ci[0].Invoke(consargs);
            }
            else
                reflectOb = ci[0].Invoke(null);       
           

            Console.WriteLine("\nВызов методов для объекта reflectOb.\n");

            // Игнорировать наследуемые методы.
            MethodInfo[] mi = t.GetMethods(
                BindingFlags.DeclaredOnly |
                BindingFlags.Instance|
                BindingFlags.Public);

            // Вызвать каждый метод.
            foreach (MethodInfo m in mi) {

                Console.WriteLine($"Вызов метода {m.Name}");
                
                // Получить параметры.
                ParameterInfo[] pi = m.GetParameters();

                // Выполнить методы.
                switch (pi.Length) {
                    case 0: //аргументы отсутсвуют.
                        if (m.ReturnType == typeof(int))
                        {
                            val = (int)m.Invoke(reflectOb, null);
                            Console.WriteLine($"Результат: {val}");
                        }
                        else if (m.ReturnType == typeof(void))
                            m.Invoke(reflectOb, null);

                        break;
                    case 1: // один аргумент.
                        if (pi[0].ParameterType == typeof(int)) {
                            object[] argparm = new object[1];
                            argparm[0] = 14;
                            if ((bool)m.Invoke(reflectOb, argparm))
                                Console.WriteLine("Значение 14 находятся между х и у");
                            else
                                Console.WriteLine("Значение 14 не находится между х и у");
                        }

                        break;
                    case 2: //два аргумента
                        if ((pi[0].ParameterType == typeof(int)) && (pi[1].ParameterType == typeof(int)))
                        {
                            object[] argparm = new object[2];
                            argparm[0] = 9;
                            argparm[1] = 18;
                            m.Invoke(reflectOb, argparm);
                        }
                        else if ((pi[0].ParameterType == typeof(double)) && (pi[1].ParameterType == typeof(double))) {
                            object[] argparm = new object[2];
                            argparm[0] = 1.12;
                            argparm[1] = 23.4;
                            m.Invoke(reflectOb, argparm);
                        }

                        break;
                }
                Console.WriteLine();                               
            }
            Console.ReadKey();
        }
    }
}
