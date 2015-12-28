using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign.PEE.Q
{
    // Changing names will invalidate all existing saves!
    enum QID
    {
        Null,
        [QuestType(typeof(Q1))]
        Beginning,
        [QuestType(typeof(Q2))]
        Something,
        [QuestType(typeof(Q3))]
        TheEnd
    }

    interface IQ
    {
        QID Id { get; }
        string Title { get; }
        string Description { get; }
    }

    class Q : IQ
    {
        public QID Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class QuestTypeAttribute : FactoryTypeAttribute
    {
        public QuestTypeAttribute(Type val) : base(val) { }
    }

    static class QFactory
    {
        public static IQ Create(QID id)
        {
            var newQ = EnumAttributeFactory.Create<QID, QuestTypeAttribute, Q>(id);
            newQ.Id = id;
            return newQ;
        }
    }

    // ----> Move to other file

    class Q1 : Q { }
    class Q2 : Q { }
    class Q3 : Q { }

    public class FactoryTypeAttribute : Attribute
    {
        public Type Value { get; private set; }

        public FactoryTypeAttribute(Type val)
        {
            Value = val;
        }
    }

    static class EnumAttributeFactory
    {
        public static TResult Create<TEnum, TAttribute, TResult>(TEnum id)
            where TResult : class, new()
            where TAttribute : Attribute
            where TEnum : struct
        {
            var attrributes = typeof(TEnum).GetMember(id.ToString())[0].GetCustomAttributes(typeof(TAttribute), false);
            if (attrributes == null)
                throw new ArgumentException("Missing type attribute for enum value : " + id);

            var type = ((QuestTypeAttribute)attrributes[0]).Value;
            var result = Activator.CreateInstance<TResult>();
            return result;
        }
    }
}
