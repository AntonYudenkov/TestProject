using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using BL.Interfaces;

namespace BL.Models
{
    [XmlRoot(ElementName="Card")]
    public class Card : IUser { 

        [XmlElement(ElementName="Pan")] 
        public string Pan { get; set; } 

        [XmlElement(ElementName="ExpDate")] 
        public string ExpDate { get; set; } 

        [XmlAttribute(AttributeName="UserId")] 
        public int UserId { get; set; } 

        [XmlText] 
        public string Text { get; set; } 
    }

    [XmlRoot(ElementName="Cards")]
    public class XmlDataModel { 

        [XmlElement(ElementName="Card")] 
        public List<Card> Cards { get; set; } 
    }

}