using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SMEAppHouse.Core.Reflections
{
    public static partial class ExpressionBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public class Filter
        {
            public string PropertyName { get; set; }
            public string Value { get; set; }
            public Type DataType { get; set; }
            public Operator Operator { get; set; } = Operator.Contains;

            /*
                var members = arg.Split(':');
                var key = members[0];
                var valRaw = members[1];

                var type = valRaw.Split('[', ']')[1].Trim();
                var value = valRaw.Replace(type, "").Replace("[", "").Replace("]", "").Trim();

                this.Add(key, value);
            */

            /// <summary>
            /// 
            /// </summary>
            public class Filters : List<Filter>
            {
                /// <summary>
                /// 
                /// </summary>
                /// <param name="filtersjson"></param>
                public Filters(string filtersjson)
                {
                    var keyFieldValueFiltersDic = JsonConvert.DeserializeObject<IDictionary<string, string>>(filtersjson);
                    foreach (var arg in keyFieldValueFiltersDic)
                    {
                        var valRaw = arg.Value;
                        var type = valRaw.Split('[', ']')[1].Trim();
                        var value = valRaw.Replace(type, "").Replace("[", "").Replace("]", "").Trim();

                        this.Add(arg.Key, value);
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="filters"></param>
                public Filters(params string[] filters)
                {
                    foreach (var arg in filters)
                    {

                        var members = arg.Split(':');
                        var key = members[0];
                        var valRaw = members[1];

                        var type = valRaw.Split('[', ']')[1].Trim();
                        var value = valRaw.Replace(type, "").Replace("[", "").Replace("]", "").Trim();

                        this.Add(key, value);
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="filters"></param>
                public Filters(IDictionary<string, string> filters)
                {
                    foreach (var f in filters)
                    {
                        var type = f.Value.Split('[', ']')[1].Trim();
                        var value = f.Value.Replace(type, "").Replace("[", "").Replace("]", "").Trim();

                        this.Add(f.Key, value);
                    }
                }




                /// <summary>
                /// 
                /// </summary>
                /// <param name="name"></param>
                /// <param name="value"></param>
                public void Add(string name, string value = "")
                {
                    this.Add(new Filter() { PropertyName = name, Value = value });
                }
            }
        }

    }

}
