using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MySerializer
{
    public class MySerializer
    {  
        public static string Serialize(Object o)
        {
            if (o.GetType().IsPrimitive) {  // if primitive  
                return o.ToString();        // return just the value 
            }

            Type type = o.GetType();
            // String serialStr = serializehelper(type, "");
            String serialStr = "{";
            foreach (FieldInfo info in type.GetFields())
            {
                if (info.MemberType == MemberTypes.Field)
                { 
                    serialStr += " { " + info.Name + " = " + Serialize(info.GetValue(o)) + " }";
                }
            }
            serialStr += " }"; 
            return serialStr;
        }
         
        public const int varPosition = 1;
        public const int valPosition = 3; 


        // resource: https://stackoverflow.com/questions/8625/generic-type-conversion-from-string
        public static object Get(string _toparse, Type _t)
        {
            // Test for Nullable<T> and return the base type instead:
            Type undertype = Nullable.GetUnderlyingType(_t);
            Type basetype = undertype == null ? _t : undertype;
            return Convert.ChangeType(_toparse, basetype);
        }

        public static T Get<T>(string _key)
        {
            return (T)Get(_key, typeof(T));
        }

        public static int findEndCurlyBraceIndex(int startIndex, string[] strArr) {
            int index = 0, curlyOccur = 0; 
            for (int i = startIndex; i < strArr.Length; i++) {
                if (strArr[i] == "{") {
                    curlyOccur++;
                } 
                if (strArr[i] == "}") {
                    curlyOccur--;
                }
                if (curlyOccur == 0) {
                    index = i;
                    break;
                }
            }
            return index;
        }
		
		public static string[] SubArray(string[] data, int index, int length)
		{
    		string [] result = new string[length];
    		Array.Copy(data, index, result, 0, length);
    		return result;
		}
		
		// Length of a base case string, e.g. [ { , x , = , 1 , } ]
		public const int baseStrLen = 5; 

        /// <exception cref="Exception"></exception>
        public static T Deserialize<T>(string str)
        {
			string[] strArr = str.Split(null);
            T obj = default(T);

            Type type = typeof(T);
			
			if (type.IsPrimitive) {
                String val = strArr[valPosition];
                obj = Get<T>(val);
                return obj; 
            } 
			
            ConstructorInfo ctor = type.GetConstructor(new Type[] { });
            obj = (T)ctor.Invoke(new Object[] { }); // Object Created
            
            // i = 1, skip the first curly brace
            for (int i = 1; (i < strArr.Length - 1); i++) {
                string varName = "";
				string [] subExpr = {};
				int startIndex = 0; 
				int endIndex = 0; 
                if (strArr[i] == "=") {
					varName = strArr[i - 1]; 
					if (strArr[i + 1] == "{") { // indicate member type is an object 
						startIndex = i + 1;
						  
                    	endIndex = findEndCurlyBraceIndex(startIndex, strArr);
						i = endIndex + 1; 
					} else {
						startIndex = i - 2; // start from the first curly brace for extracting sub-string 
						endIndex = findEndCurlyBraceIndex(startIndex, strArr);
						i = endIndex + 1; 
						 
					}
					// extracting the value strin of member type
					subExpr = SubArray(strArr, startIndex, endIndex - startIndex + 1);
                }
				 
				
				if (subExpr.Length >= baseStrLen) {
					//Console.WriteLine("SubExpression: " + string.Join(" ", subExpr)); 
					Regex re = new Regex("^[_a-z]\\w*");
					if (re.IsMatch(varName)) {
						 FieldInfo info = type.GetField(varName);
					     //Console.WriteLine("Varname: " + varName);
					 
					 if (info != null) {
						// abtaining member field type 
						Type t = info.FieldType; 
						//Console.WriteLine("Type: " + info.FieldType);
						MethodInfo method = typeof(MySerializer).GetMethod("Deserialize")
                             .MakeGenericMethod(new Type[] { t });
						String substr = string.Join(" ", subExpr);
						//Console.WriteLine("string :" +  substr + " length :" + subExpr.Length);
						// recursively creating member field type object and assign values  
						var memberObj = method.Invoke(null, new object[] { substr });
				
						info.SetValue(obj, memberObj);
					 }
					 
					}
				   
				}
            } 
            return obj;
        }

}

	public class Point
    {
        public int x, y;
        public Point()
        {
            x = y = 0;
        }
        public Point(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public override string ToString()
        {
            return String.Format("Point = ({0},{1})", x, y);
        }
    }

	public class Rectangle
    {
        public Point topleft, bottomRight;
        public Rectangle()
        {
            topleft = new Point(0, 0);
            bottomRight = new Point(0, 0);
        }
        public Rectangle(Point tl, Point br)
        {
            topleft = tl;
            bottomRight = br;
        }
        public override string ToString()
        {
            return String.Format("Rectangle = (TopLeft Point = {0}, BottomRight Point = {1})", topleft, bottomRight);
        }
    }
	
	public class Circle {
		public Point origin;
		public double radius; 
		public Circle() {
			origin = new Point(0,0);
			radius = 0.0; 
		} 
		public Circle(Point origin, double radius) {
			this.origin = origin;
			this.radius = radius; 
		}
		public override string ToString() {
			return String.Format("Circle = (Origin = ({0}), Radius = {1})", origin, radius);	
		}
	}
	
	public class Student {
		public bool gender; 
		public int id; 
		public Student() {
			gender = true; 
		    id = 0; 
		}
		public Student(bool gender, int id) {
			this.gender = gender; 
			this.id = id; 
		}
		public override string ToString() {
			return String.Format("Student = (gender = {0}, id = {1})", gender, id);
		}
	}


    public class Test
    {
        public static void Main(String[] args)
        { 
            Point p1 = new Point(2, 3);
            String str1 = MySerializer.Serialize(p1);
            Console.WriteLine(str1);
            Point newPt = MySerializer.Deserialize<Point>(str1);
            Console.WriteLine(newPt);
        }
    }
}