using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConnectedData.LinkedIn.Service
{

    internal class ConnectionsDeserializer : IMapper<string, IEnumerable<LinkedInPersonDto>>, IMapper<XDocument, IEnumerable<LinkedInPersonDto>>
    {

        public IEnumerable<LinkedInPersonDto> Map(string t)
        {
            if (string.IsNullOrEmpty(t)) return new List<LinkedInPersonDto>();
            return Map(XDocument.Parse(t));
        }

        public IEnumerable<LinkedInPersonDto> Map(XDocument doc)
        {
            var maps = new List<LinkedInPersonDto>();
            
            foreach (var personElement in doc.Root.Elements("person").Where(e => !e.Element("first-name").Value.Equals("private")))
            {
                var id = personElement.Element("id").Value;
                var firstName = GetValueFromElement(personElement,"first-name");
                var lastName = GetValueFromElement(personElement, "last-name");
                var headline = GetValueFromElement(personElement, "headline");
                var industry = GetValueFromElement(personElement, "industry");
                var location = GetLocation(personElement);
                var countryCode = GetCountryCode(personElement);
                
                
                var pictureUriValue = GetValueFromElement(personElement, "picture-url");
                Uri pictureUri = pictureUriValue == String.Empty ? null : new Uri(pictureUriValue);

                maps.Add(new LinkedInPersonDto()
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Headline = headline,
                    Industry = industry,
                    Location = location,
                    CountryCode = countryCode,
                    PictureUrl = pictureUri
                });
            }
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
