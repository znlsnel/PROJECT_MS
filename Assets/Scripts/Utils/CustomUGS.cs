using GoogleSheet.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;


namespace Hamster.ZG.Type
{
    public static class UGSUtill
    {
        public static System.Type FindEnumType(string typeName)
        {
            // 프로젝트의 모든 어셈블리에서 타입 찾기
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(typeName);
                if (type != null && type.IsEnum)
                    return type;
                
                // 네임스페이스가 없는 경우도 처리
                type = assembly.GetType($"YourNamespace.{typeName}");
                if (type != null && type.IsEnum)
                    return type;
            }
            return null;
        }
        public static Enum ParseEnum(System.Type enumType, string enumValue)
        {
            if (Enum.TryParse(enumType, enumValue, out object enumResult))
                return (Enum)enumResult;
            return null;
        }

        public static string[] SplitList(string input)
        {
            // 입력 문자열의 앞뒤 공백 제거
            input = input.Trim();
            
            // 첫번째 형식인 경우 (튜플 리스트 형식)
            if (input.StartsWith("[") && input.Contains("],"))
            {
                // 정규식으로 각 [...] 부분 추출
                var matches = Regex.Matches(input, @"\[(.*?)\]");
                var result = new List<string>();
                
                foreach (Match match in matches)
                {
                    result.Add(match.Groups[0].Value); // 전체 매칭(괄호 포함) 추가 
                }
                
                return result.ToArray();
            }
            // 두번째 형식인 경우 (쉼표로 구분된 리스트 형식)
            else if (input.Contains(","))
            {
                // 쉼표로 분리하고 각 요소의 공백 제거
                var parts = input.Split(',');
                var result = new List<string>();
                
                foreach (var part in parts)
                {
                    result.Add(part.Trim());
                }
                
                return result.ToArray();
            }
            // 두 형식 모두 아닌 경우
            else
            {
                return new string[] { input };
            }


        }
        public static string[] SplitPair(string value)
        {
            value = value.Trim('[', ']'); 
            return value.Split(',')
                        .Select(v => v.Trim())
                        .Where(v => !string.IsNullOrEmpty(v))
                        .ToArray();
        }
    }

    [Type(typeof(ValueTuple<EStatType, int>), new string[] { "(EStatType,Int)", "(EStatType, Int)"})] 
    public class EnumIntPairType : IType
    {
        public object DefaultValue => new Tuple<EStatType, int>(EStatType.Str, 0);

        public object Read(string value)
        {
            var tuple = UGSUtill.SplitPair(value);
            string enumTypeName = tuple[0].Trim();
            string enumValueName = tuple[1].Trim();

            // Enum 타입 찾기
            var temp = UGSUtill.ParseEnum(typeof(EStatType), enumTypeName);
            if (temp == null)
                throw new FormatException($"Could not parse {enumTypeName} to EStatType");

            EStatType statType = (EStatType)temp;

            // int 값 파싱
            if (!int.TryParse(enumValueName, out int intValue))
                throw new FormatException($"Invalid integer value: {enumValueName}");

            // Enum 값을 Type으로 변환
            return new ValueTuple<EStatType, int>(statType, intValue);
        }

        public string Write(object value)
        {
            Tuple<EStatType, int> pair = (Tuple<EStatType, int>)value;
            return $"{pair.Item1}:{pair.Item2}";
        }

        
    } 

    [Type(typeof(List<(EStatType, int)>), new string[] { "List<(EStatType,Int)>"})]
    public class EnumIntListType : IType
    {
        public object DefaultValue => new List<(EStatType, int)>();

        public object Read(string value)
        {
            var list = new List<(EStatType, int)>();

            // 각 [Enum,Int] 쌍 분리
            var myList = UGSUtill.SplitList(value);

            foreach (var pair in myList)
            {
                var tuple = UGSUtill.SplitPair(pair);
                string enumTypeName = tuple[0].Trim();
                string intValue = tuple[1].Trim();

                var temp = UGSUtill.ParseEnum(typeof(EStatType), enumTypeName);
                if (temp == null)
                    throw new FormatException($"Could not parse {enumTypeName} to EStatType");

                EStatType statType = (EStatType)temp;
                
                // int 값 파싱
                if (!int.TryParse(intValue, out int numericValue))
                    throw new FormatException($"Invalid integer value: {intValue}");

                list.Add((statType, numericValue));
            }

            return list;
        }

        public string Write(object value)
        {
            var list = (List<(System.Type, int)>)value;
            var pairs = list.Select(pair => $"[{pair.Item1.FullName}, {pair.Item2}]");
            return string.Join(", ", pairs);
        }

    }
 
    [Type(typeof(List<EStatType>), new string[] { "List<EStatType>" })]
    public class EnumListType : IType
    {
        public object DefaultValue => new List<EStatType>();

        public object Read(string value) 
        {
            var list = new List<EStatType>();
            
            try
            {
                var mylist = UGSUtill.SplitList(value); 
                foreach (var item in mylist)
                {
                    var temp = UGSUtill.ParseEnum(typeof(EStatType), item);
                    if (temp == null)
                        throw new FormatException($"Could not parse {item} to EStatType");
                    list.Add((EStatType)temp);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing Enum list: {e.Message} in value: {value}");
                return DefaultValue;
            }

            return list;
        }

    
        public string Write(object value)
        {
            var list = (List<EStatType>)value;
            if (list.Count == 0)
                return string.Empty;

            System.Type enumType = list[0].GetType();
            var values = list.Select(e => e.ToString());
            return $"[{string.Join(",", values)}]";
        }
    }


    [Type(typeof(bool), new string[] { "bool" })]
    public class BoolType : IType
    {
        public object DefaultValue => false;

        public object Read(string value)
        {
            return bool.Parse(value);
        } 

        public string Write(object value)
        {
            return value.ToString();
        }
    }

}
