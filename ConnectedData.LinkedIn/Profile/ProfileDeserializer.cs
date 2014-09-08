using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConnectedData.LinkedIn.Service
{

    public class ProfileDeserializer : IMapper<string, DetailedPersonDto>, IMapper<XDocument, DetailedPersonDto>
    {

        public DetailedPersonDto Map(string t)
        {
            if (string.IsNullOrEmpty(t)) return new DetailedPersonDto();
            return Map(XDocument.Parse(t));
        }

        public DetailedPersonDto Map(XDocument doc)
        {
            var map = new DetailedPersonDto();
            var personElement = doc.Root;
            map.FirstName = GetValueFromElement(personElement, "first-name");
            map.LastName = GetValueFromElement(personElement, "last-name");
            map.Headline = GetValueFromElement(personElement, "headline");
            map.Id = GetValueFromElement(personElement, "id");
            map.Industry = GetValueFromElement(personElement, "industry");
            map.Location = GetLocation(personElement);
            map.CountryCode = GetCountryCode(personElement);
            map.Educations = GetEducations(personElement);
            map.Skills = GetSkills(personElement,"skills");
            map.Positions = GetPositions(personElement, "positions");
            return map;
        }

        private IEnumerable<PositionDto> GetPositions(XElement personElement, string positionElementName)
        {
            var positions = new List<PositionDto>();
            var positionsElement = personElement.Element(positionElementName);
            if (null == positionsElement) return positions;
            var positionElements = positionsElement.Elements("position");
            if (null != positionElements && 0 < positionElements.Count())
            {
                foreach (var positionElement in positionElements)
                {
                    var position = new PositionDto();
                    position.Id = GetValueFromElement(positionElement, "id");
                    position.Company = GetCompany(positionElement.Element("company"));
                    position.EndDate = GetEndDate(positionElement, "end-date");
                    position.StartDate = GetStartDate(positionElement, "start-date");
                    var isCurrent = false;
                    if (bool.TryParse(GetValueFromElement(positionElement,"is-current"),out isCurrent))
                    {
                        position.IsCurrent = isCurrent;
                    }
                    position.Summary = GetValueFromElement(positionElement, "summary");
                    position.Title = GetValueFromElement(positionElement, "summary");
                    positions.Add(position);
                }
            }
            
            return positions;
        }

        private CompanyDto GetCompany(XElement xElement)
        {
            return new CompanyDto()
            {
                Id = GetValueFromElement(xElement, "id"),
                Name = GetValueFromElement(xElement, "name")
            };
        }

        

        private IEnumerable<string> GetSkills(XElement personElement, string skillsElementName)
        {
            var skills = new List<string>();
            var skillElement = personElement.Element(skillsElementName);
            if (null == skillElement) return skills;
            var skillElements = skillElement.Elements("skill");

            if (null != skillElements && 0 < skillElements.Count())
            {
                foreach(var element in skillElements)
                {
                    if (null != element.Element("skill"))
                        if (null != element.Element("skill").Element("name"))
                        {
                            var name = GetValueFromElement(element.Element("skill"), "name");
                            if (!string.IsNullOrWhiteSpace(name))
                                skills.Add(name);
                        }
                    
                }
            }
            
            return skills;
        }

        private IEnumerable<EducationDto> GetEducations(XElement personElement)
        {
            var educations = new List<EducationDto>();
            var educationsElement = personElement.Element("educations");
            foreach (var educationElement in educationsElement.Elements("education"))
                educations.Add(GetEducation(educationElement));
            return educations;
        }

        private EducationDto GetEducation(XElement educationElement)
        {
            var education = new EducationDto();
            education.Degree = GetValueFromElement(educationElement,"degree");
            education.FieldOfStudy = GetValueFromElement(educationElement, "field-of-study");
            education.Id = GetValueFromElement(educationElement, "id");
            education.StartDate = GetStartDate(educationElement, "start-date");
            education.EndDate = GetEndDate(educationElement, "end-date");
            education.SchoolName = GetValueFromElement(educationElement, "school-name");
            return education;
        }

        private DateTime? GetStartDate(XElement element, string childElementName)
        {
            DateTime? startDate = GetDate(element.Element(childElementName));
            if (startDate.HasValue)
                return new DateTime(startDate.Value.Year, startDate.Value.Month, 1);
            else return startDate;
        }

        private DateTime? GetEndDate(XElement element, string childElementName)
        {
            DateTime? endDate = GetDate(element.Element(childElementName));
            if (endDate.HasValue)
            {
                if (null == element.Element(childElementName).Element("month"))
                    endDate = new DateTime(endDate.Value.Year, 12, 31);
                else
                    endDate = new DateTime(endDate.Value.Year, endDate.Value.Month + 1, 1).AddDays(-1);
            }
                
            return endDate;
        }

        private DateTime? GetDate(XElement element)
        {
            DateTime? date = null;
            var startDateElement = element;
            if (null == startDateElement) return date;
            var yearElement = startDateElement.Element("year");
            if (null == yearElement) return date;
            var year = 0;
            if (Int32.TryParse(yearElement.Value, out year))
                date = new DateTime(year, 1, 1);
            else return date;

            var monthElement = startDateElement.Element("month");
            if (null == monthElement) return date;
            var month = 0;
            if (Int32.TryParse(monthElement.Value, out month))
                date.Value.AddMonths(month);
            return date;
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
