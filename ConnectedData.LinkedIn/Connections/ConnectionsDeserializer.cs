﻿using ConnectedData.DataTransfer;
using ConnectedData.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConnectedData.LinkedIn.Connections
{

    public class ConnectionsDeserializer : IMapper<string, IEnumerable<PersonDto>>, IMapper<XDocument, IEnumerable<PersonDto>>
    {

        public IEnumerable<PersonDto> Map(string t)
        {
            if (string.IsNullOrEmpty(t)) return new List<PersonDto>();
            XDocument doc = new XDocument();
            try
            {
                doc = XDocument.Parse(t);
            }
            catch
            {
                return new List<PersonDto>();
            }
            return Map(doc);
        }

        public IEnumerable<PersonDto> Map(XDocument doc)
        {
            var maps = new List<PersonDto>();

            try
            {
                foreach (var personElement in doc.Root.Elements("person").Where(e => !e.Element("first-name").Value.Equals("private")))
                {
                    var id = personElement.Element("id").Value;
                    var firstName = GetValueFromElement(personElement, "first-name");
                    var lastName = GetValueFromElement(personElement, "last-name");
                    var headline = GetValueFromElement(personElement, "headline");
                    var industry = GetValueFromElement(personElement, "industry");
                    var location = GetLocation(personElement);
                    var countryCode = GetCountryCode(personElement);


                    var pictureUriValue = GetValueFromElement(personElement, "picture-url");
                    Uri pictureUri = pictureUriValue == String.Empty ? null : new Uri(pictureUriValue);

                    maps.Add(new PersonDto()
                    {
                        Id = id,
                        //FirstName = firstName,
                        //LastName = lastName,
                        //Headline = headline,
                        Industry = industry,
                        Location = location,
                        CountryCode = countryCode,
                        //PictureUrl = pictureUri
                    });
                }
            }
            catch { } //TO DO LOG
            return maps;
        }

        private string GetCountryCode(XElement personElement)
        {
            var locationElement = personElement.Element("location");
            if (null == locationElement) return string.Empty;
            return GetValueFromElement(locationElement.Element("country"), "code");
        }

        private string GetLocation(XElement personElement)
        {
            return GetValueFromElement(personElement.Element("location"),"name");
        }

        private string GetValueFromElement(XElement element, string elementName)
        {
            return GetValueFromElement(element.Element(elementName),NullValueBehavior.EmptyString);
        }

        private enum NullValueBehavior
        {
            Null,
            EmptyString
        }
        private string GetValueFromElement(XElement element, NullValueBehavior behavior)
        {
            string result;
            if (null == element)
                result = null;
            else
                result = element.Value;

            if (null == result)
                return behavior == NullValueBehavior.EmptyString ? string.Empty : null;
            return result;
        }

        
    }
}
