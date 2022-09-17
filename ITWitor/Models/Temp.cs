using System.Xml.Serialization;

namespace ITWitor.Models
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Товары));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Товары)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "БазоваяЕдиница")]
    public class БазоваяЕдиница
    {

        [XmlAttribute(AttributeName = "Код")]
        public int Код { get; set; }

        [XmlAttribute(AttributeName = "НаименованиеПолное")]
        public string НаименованиеПолное { get; set; }

        [XmlAttribute(AttributeName = "МеждународноеСокращение")]
        public string МеждународноеСокращение { get; set; }
    }

    [XmlRoot(ElementName = "Группы")]
    public class Groups
    {
        [XmlElement(ElementName = "Ид")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "СтавкаНалога")]
    public class СтавкаНалога
    {

        [XmlElement(ElementName = "Наименование")]
        public string Наименование { get; set; }

        [XmlElement(ElementName = "Ставка")]
        public int Ставка { get; set; }
    }

    [XmlRoot(ElementName = "СтавкиНалогов")]
    public class СтавкиНалогов
    {

        [XmlElement(ElementName = "СтавкаНалога")]
        public СтавкаНалога СтавкаНалога { get; set; }
    }

    [XmlRoot(ElementName = "ЗначениеРеквизита")]
    public class ЗначениеРеквизита
    {

        [XmlElement(ElementName = "Наименование")]
        public string Наименование { get; set; }

        [XmlElement(ElementName = "Значение")]
        public string Значение { get; set; }
    }

    [XmlRoot(ElementName = "ЗначенияРеквизитов")]
    public class ЗначенияРеквизитов
    {

        [XmlElement(ElementName = "ЗначениеРеквизита")]
        public List<ЗначениеРеквизита> ЗначениеРеквизита { get; set; }
    }

    [XmlRoot(ElementName = "Товар")]
    public class Товар
    {

        [XmlElement(ElementName = "Ид")]
        public string Ид { get; set; }

        [XmlElement(ElementName = "Штрихкод")]
        public double Штрихкод { get; set; }

        [XmlElement(ElementName = "Артикул")]
        public int Артикул { get; set; }

        [XmlElement(ElementName = "Код")]
        public string Код { get; set; }

        [XmlElement(ElementName = "Наименование")]
        public string Наименование { get; set; }

        [XmlElement(ElementName = "БазоваяЕдиница")]
        public БазоваяЕдиница БазоваяЕдиница { get; set; }

        [XmlElement(ElementName = "Группы")]
        public Groups Группы { get; set; }

        [XmlElement(ElementName = "Категория")]
        public string Категория { get; set; }

        [XmlElement(ElementName = "Описание")]
        public object Описание { get; set; }

        [XmlElement(ElementName = "Страна")]
        public string Страна { get; set; }

        [XmlElement(ElementName = "СтавкиНалогов")]
        public СтавкиНалогов СтавкиНалогов { get; set; }

        [XmlElement(ElementName = "ЗначенияРеквизитов")]
        public ЗначенияРеквизитов ЗначенияРеквизитов { get; set; }

        [XmlElement(ElementName = "Вес")]
        public DateTime Вес { get; set; }

        [XmlElement(ElementName = "ЗначенияСвойств")]
        public ЗначенияСвойств ЗначенияСвойств { get; set; }

        [XmlAttribute(AttributeName = "Статус")]
        public string Статус { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ЗначенияСвойства")]
    public class ЗначенияСвойства
    {

        [XmlElement(ElementName = "Ид")]
        public string Ид { get; set; }

        [XmlElement(ElementName = "Значение")]
        public object Значение { get; set; }
    }

    [XmlRoot(ElementName = "ЗначенияСвойств")]
    public class ЗначенияСвойств
    {

        [XmlElement(ElementName = "ЗначенияСвойства")]
        public ЗначенияСвойства ЗначенияСвойства { get; set; }
    }

    [XmlRoot(ElementName = "Товары")]
    public class Товары
    {
        [XmlElement(ElementName = "Товар")]
        public List<Товар> Товар { get; set; }
    }


    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(КоммерческаяИнформация));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (КоммерческаяИнформация)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "Владелец")]
    public class Owner
    {

        [XmlElement(ElementName = "Ид")]
        public string Id { get; set; }

        [XmlElement(ElementName = "Наименование")]
        public string Name { get; set; }

        [XmlElement(ElementName = "ОфициальноеНаименование")]
        public string OfficialName { get; set; }

        [XmlElement(ElementName = "ОКПО")]
        public object OKPO { get; set; }
    }

    [XmlRoot(ElementName = "Налог")]
    public class Tax
    {

        [XmlElement(ElementName = "Наименование")]
        public string Name { get; set; }

        [XmlElement(ElementName = "УчтеноВСумме")]
        public bool IncludeWithPrice { get; set; }
    }

    [XmlRoot(ElementName = "ТипЦены")]
    public class PriceType
    {
        [XmlElement(ElementName = "Ид")]
        public string Id { get; set; }

        [XmlElement(ElementName = "Наименование")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Валюта")]
        public string Currency { get; set; }

        [XmlElement(ElementName = "Налог")]
        public Tax Tax { get; set; }
    }

    [XmlRoot(ElementName = "Цена")]
    public class Price
    {
        [XmlElement(ElementName = "Представление")]
        public string Представление { get; set; }

        [XmlElement(ElementName = "ИдТипаЦены")]
        public string ИдТипаЦены { get; set; }

        [XmlElement(ElementName = "ЦенаЗаЕдиницу")]
        public int ЦенаЗаЕдиницу { get; set; }

        [XmlElement(ElementName = "Валюта")]
        public string Валюта { get; set; }

        [XmlElement(ElementName = "Единица")]
        public int Единица { get; set; }

        [XmlElement(ElementName = "Коэффициент")]
        public int Коэффициент { get; set; }
    }

    [XmlRoot(ElementName = "Предложение")]
    public class Offer
    {
        [XmlElement(ElementName = "Ид")]
        public string Id { get; set; }

        [XmlElement(ElementName = "Наименование")]
        public string Name { get; set; }

        [XmlElement(ElementName = "БазоваяЕдиница")]
        public БазоваяЕдиница БазоваяЕдиница { get; set; }

        [XmlElement(ElementName = "Штрихкод")]
        public double Штрихкод { get; set; }

        [XmlElement(ElementName = "Артикул")]
        public int Article { get; set; }

        [XmlElement(ElementName = "Цены")]
        public List<Price> Prices { get; set; }

        [XmlElement(ElementName = "Количество")]
        public int Count { get; set; }

        [XmlAttribute(AttributeName = "Статус")]
        public string State { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ПакетПредложений")]
    public class ПакетПредложений
    {
        [XmlElement(ElementName = "Ид")]
        public string Ид { get; set; }

        [XmlElement(ElementName = "Наименование")]
        public string Наименование { get; set; }

        [XmlElement(ElementName = "ИдКаталога")]
        public string ИдКаталога { get; set; }

        [XmlElement(ElementName = "ИдКлассификатора")]
        public string ИдКлассификатора { get; set; }

        [XmlElement(ElementName = "Владелец")]
        public Owner Owner { get; set; }

        [XmlElement(ElementName = "ТипыЦен")]
        public List<PriceType> PriceTypes { get; set; }

        [XmlElement(ElementName = "Предложения")]
        public List<Offer> Offer { get; set; }

        [XmlAttribute(AttributeName = "СодержитТолькоИзменения")]
        public bool СодержитТолькоИзменения { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "КоммерческаяИнформация")]
    public class КоммерческаяИнформация
    {

        [XmlElement(ElementName = "ПакетПредложений")]
        public ПакетПредложений ПакетПредложений { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "xs")]
        public string Xs { get; set; }

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "ВерсияСхемы")]
        public DateTime ВерсияСхемы { get; set; }

        [XmlAttribute(AttributeName = "ДатаФормирования")]
        public DateTime ДатаФормирования { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
