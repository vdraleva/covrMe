using CovrMe.Models;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CovrMe.Services.Implementation
{
    public partial class RegExtractorRegexService : IRegExtractorRegexService
    {
        #region Fields
        private readonly IOcrService _ocrService;
        #endregion

        #region Props
        private RegCertificateResultModel vehicleModel = new RegCertificateResultModel();
        #endregion

        public RegExtractorRegexService(IOcrService ocrService)
        {
            _ocrService = ocrService;
        }

        #region Regex

        [GeneratedRegex(@"\s([A-ZА-Я]{1,2}\d{4}[A-ZА-Я]{1,2})\|?\s")]
        private static partial Regex _carPlateRegex();

        [GeneratedRegex(@"\s([A-Z0-9)]{13}[0-9]{4}).*\s")]
        private static partial Regex _VINRegex();

        [GeneratedRegex(@"[^0-9]([0-9]{9})[^0-9]")]
        private static partial Regex _documentNumberRegex();

        [GeneratedRegex(@"\s+([0-9]{10})\s+")]
        private static partial Regex _uinRegex();

        [GeneratedRegex(@"(?:\(\s*[B8]\s*\)|\(\s*F\s*.\s*1\s*\)).*?([0-9]{2}\.[0-9]{2}\.[0-9]{4})")]
        private static partial Regex _firstRegistrationDateRegex();

        [GeneratedRegex(@"(?:об[шщ]\.)\s*\.*\s+([А-Я- ]{3,})")]
        private static partial Regex _municipalityRegex();

        [GeneratedRegex(@"Обл\..*?\s+([А-Я- ]{3,})")]
        private static partial Regex _regionRegex();

        [GeneratedRegex(@"(?:Обл\.|общ\.).*?(?:ГР\.|С\.)\s*([А-Я- ]{3,})")]
        private static partial Regex _cityRegex();

        [GeneratedRegex(@"(?:(?:Ж\.К\.)|(?:УЛ\.)).*?\s*([A-ZА-Я- ]{3,})")]
        private static partial Regex _streetOrHoodRegex();

        [GeneratedRegex(@"(?:УЛ\.|Ж\.K\.).*?(?:бл\.\/).*?:\s([0-9]+[A-ZА-Я]*)")]
        private static partial Regex _blockNumberRegex();

        [GeneratedRegex(@"(?:вх:)\s?([А-Я]+),")]
        private static partial Regex _enteranceRegex();

        [GeneratedRegex(@"(?:ет:|et:)\s?([0-9]+)")]
        private static partial Regex _floorNumberRegex();

        [GeneratedRegex(@"(?:ап:|ap|ар:)\s?([0-9]+)")]
        private static partial Regex _apartmentNumberRegex();

        [GeneratedRegex(@"EURO\s*[\dS&][\s\S]*?([А-Я-]{3,})+[\s\S]*?\d{5,}[\s\S]*?([А-Я-]{3,})[\s\S]*?([А-Я-]{3,})[\s\S]*?OWNER")]
        private static partial Regex _fullNameRegex();

        [GeneratedRegex(@"([А-Я-]{3,})[\s\S]*?([А-Я-]{3,})[\s\S]*?([А-Я-]{3,})")]
        private static partial Regex _fullNameSmallDocRegex();

        [GeneratedRegex(@"БЕНЗИН|ГАЗ|МЕТАН|ДИЗЕЛ|ЕЛЕКТРИЧЕСКИ|ХИБРИДЕН")]
        private static partial Regex _engineType();

        [GeneratedRegex(@"ЛЕК АВТОМОБИЛ|АВТОБУС|ТОВАРЕН АВТОМОБИЛ|МОТОЦИКЛЕТ|СЕЛСКОСТОПАНСКИ|СТРОИТЕЛЕН|КАРИ\/ВЗТ|КАРАВАН\/БАГАЖНО РЕМАРКЕ|ТОВАРНО РЕМАРКЕ|СЕДЛОВ ВЛЕКАЧ")]
        private static partial Regex _vehicleTypeRegex();

        [GeneratedRegex(@"\([РP]\s*\.*\s*2\)\s?([0-9]+)")]
        private static partial Regex _maxNetPowerKwRegex();

        [GeneratedRegex(@"\(*[РP]\s*\.*\s*1\)\s?([0-9]+)")]
        private static partial Regex _engineVolume();

        [GeneratedRegex(@"\s([1-9]{1,2})[+4]1\|?\s\(")]
        private static partial Regex _placesRegex();

        [GeneratedRegex(@"\(R\)\s?([А-Я]+)")]
        private static partial Regex _colorRegex();

        [GeneratedRegex(@"\(F\s*\.*\s*[12]\)\s?([0-9]+)")]
        private static partial Regex _grossWeightRegex();

        [GeneratedRegex(@"\(G\)\s?([0-9]+)")]
        private static partial Regex _netWeightRegex();

        private static Regex _vehicleMakeModelRegex;
        private static Regex GetVehicleMakeModelRegex()
        {
            if (_vehicleMakeModelRegex == null)
            {
                string pattern = "(" + string.Join("|", VehicleMakesHelper.Makes) + ")" + " +([A-Z0-9- ]+)";
                _vehicleMakeModelRegex = new Regex(pattern);
            }
            return _vehicleMakeModelRegex;
        }
        #endregion

        #region Methods
        public async Task<RegCertificateResultModel> ParseToModel(byte[] image, DocumentTypeEnum docType)
        {
            vehicleModel = new RegCertificateResultModel();
            string ocrTextOutput = String.Empty;
            try
            {
                ocrTextOutput = await _ocrService.RecognizeImage(image);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var carPlateMatch = _carPlateRegex().Match(ocrTextOutput);
            if (carPlateMatch.Success)
            {
                var carPlate = LatinToCyrillicLetters(carPlateMatch.Groups[1].Value);
                vehicleModel.PlateNumber = carPlate;
            }

            var VINMatch = _VINRegex().Match(ocrTextOutput);
            if (VINMatch.Success)
            {
                vehicleModel.Vin = VINMatch.Groups[1].Value;
            }

            var documentNumberMatch = _documentNumberRegex().Match(ocrTextOutput);
            if (documentNumberMatch.Success)
            {
                vehicleModel.RegistrationCertificateNumber = documentNumberMatch.Groups[1].Value;
            }

            var firstRegistrationDateMatch = _firstRegistrationDateRegex().Match(ocrTextOutput);
            if (firstRegistrationDateMatch.Success)
            {
                vehicleModel.FirstRegistrationDate = DateTime.ParseExact(firstRegistrationDateMatch.Groups[1].Value, "dd.MM.yyyy", null);
            }

            var vehicleInfoMatch = GetVehicleMakeModelRegex().Match(ocrTextOutput);
            if (vehicleInfoMatch.Success)
            {
                vehicleModel.Brand = vehicleInfoMatch.Groups[1].Value;
                vehicleModel.Model = vehicleInfoMatch.Groups[2].Value;
            }

            var maxNetPowerMatch = _maxNetPowerKwRegex().Match(ocrTextOutput);
            if (maxNetPowerMatch.Success)
            {
                vehicleModel.VehicleKilowatts = maxNetPowerMatch.Groups[1].Value;
            }

            var engineCapacityMatch = _engineVolume().Match(ocrTextOutput);
            if (engineCapacityMatch.Success)
            {
                vehicleModel.EngineVolume = (engineCapacityMatch.Groups[1].Value);
            }

            var engineTypeMatch = _engineType().Match(ocrTextOutput);
            if (engineTypeMatch.Success)
            {
                vehicleModel.EngineType = engineTypeMatch.Value;
            }

            var vehicleTypeMatch = _vehicleTypeRegex().Match(ocrTextOutput);
            if (vehicleTypeMatch.Success)
            {
                vehicleModel.VehicleType = vehicleTypeMatch.Value;
            }

            var placesMatch = _placesRegex().Match(ocrTextOutput);
            if (placesMatch.Success)
            {
                vehicleModel.Places = (int.Parse(placesMatch.Groups[1].Value)+1).ToString();
            }

            var municipalityMatch = _municipalityRegex().Match(ocrTextOutput);
            if (municipalityMatch.Success)
            {
                vehicleModel.Municipality = AdaptLetterCase(municipalityMatch.Groups[1].Value);
            }

            var regionMatch = _regionRegex().Match(ocrTextOutput);
            if (regionMatch.Success)
            {
                vehicleModel.Region = AdaptLetterCase(regionMatch.Groups[1].Value);
            }

            var cityMatch = _cityRegex().Match(ocrTextOutput);
            if (cityMatch.Success)
            {
                vehicleModel.City = AdaptLetterCase(cityMatch.Groups[1].Value);
            }

            var streetOrHoodMatch = _streetOrHoodRegex().Match(ocrTextOutput);
            if (streetOrHoodMatch.Success)
            {
                var address = streetOrHoodMatch.Groups[0].Value;

                if (streetOrHoodMatch.Groups[0].Value.Contains("Ж.К."))
                {
                    address = "ж.к. ";
                }
                else
                {
                    address = "ул. ";
                }

                vehicleModel.Address = address + AdaptLetterCase(streetOrHoodMatch.Groups[1].Value);
            }

            var blockNumberMatch = _blockNumberRegex().Match(ocrTextOutput);
            if (blockNumberMatch.Success)
            {
                vehicleModel.Address += ", бл. " + blockNumberMatch.Groups[1].Value;
            }

            var enteranceMatch = _enteranceRegex().Match(ocrTextOutput);
            if (enteranceMatch.Success)
            {
                vehicleModel.Address += ", вх. " + enteranceMatch.Groups[1].Value;
            }

            var floorNumberMatch = _floorNumberRegex().Match(ocrTextOutput);
            if (floorNumberMatch.Success)
            {
                vehicleModel.Address += ", ет. " + floorNumberMatch.Groups[1].Value;
            }

            var apartmentNumberMatch = _apartmentNumberRegex().Match(ocrTextOutput);
            if (apartmentNumberMatch.Success)
            {
                vehicleModel.Address += ", ап. " + apartmentNumberMatch.Groups[1].Value;
            }

            var uinMatch = _uinRegex().Match(ocrTextOutput);
            if (uinMatch.Success)
            {
                vehicleModel.Uin = uinMatch.Groups[1].Value;
            }

            var fullNameMatch = _fullNameRegex().Match(ocrTextOutput);
            if (fullNameMatch.Success)
            {
                vehicleModel.FirstName = AdaptLetterCase(fullNameMatch.Groups[2].Value);
                vehicleModel.Surname = AdaptLetterCase(fullNameMatch.Groups[3].Value);
                vehicleModel.LastName = AdaptLetterCase(fullNameMatch.Groups[1].Value);
            }
            else
            {
                var fullNameSmallDocMatch = _fullNameSmallDocRegex().Match(ocrTextOutput);
                if (fullNameSmallDocMatch.Success && docType == DocumentTypeEnum.Small)
                {
                    if (IsFront(vehicleModel))
                    {
                        vehicleModel.FirstName = AdaptLetterCase(fullNameSmallDocMatch.Groups[1].Value);
                        vehicleModel.Surname = AdaptLetterCase(fullNameSmallDocMatch.Groups[2].Value);
                        vehicleModel.LastName = AdaptLetterCase(fullNameSmallDocMatch.Groups[3].Value);
                    }
                }
            }

            var colorMatch = _colorRegex().Match(ocrTextOutput);
            if (colorMatch.Success)
            {
                vehicleModel.Color = colorMatch.Groups[1].Value;
            }

            var netWeightMatch = _netWeightRegex().Match(ocrTextOutput);
            if (netWeightMatch.Success)
            {
                vehicleModel.NetWeight = netWeightMatch.Groups[1].Value;
            }

            var grossWeightMatch = _grossWeightRegex().Match(ocrTextOutput);
            if (grossWeightMatch.Success)
            {
                vehicleModel.GrossWeight = grossWeightMatch.Groups[1].Value;
            }

            if (vehicleModel.Address != null)
            {
                if (vehicleModel.Address.Length > 3 && vehicleModel.Address[0] == ',' && vehicleModel.Address[1] == ' ')
                {
                    vehicleModel.Address = vehicleModel.Address.Remove(0, 2);
                }
            }

            return vehicleModel;
        }

        public bool IsFront(RegCertificateResultModel model)
        {
            if (model.FirstRegistrationDate != null ||
                model.GrossWeight != null ||
                model.NetWeight != null ||
                model.EngineVolume != null ||
                model.EngineType != null ||
                model.Brand != null ||
                model.Model != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsPhotoBad(RegCertificateResultModel vehicleModel, DocumentTypeEnum documentType)
        {
            int recognizedFieldsCounter = 0;

            if (vehicleModel.PlateNumber != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.Brand != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.Model != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.RegistrationCertificateNumber != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.FirstRegistrationDate != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.EngineType != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.EngineVolume != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.NetWeight != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.GrossWeight != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.Places != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.VehicleKilowatts != null)
            {
                recognizedFieldsCounter++;
            }

            if (vehicleModel.Vin != null)
            {
                recognizedFieldsCounter++;
            }

            if (documentType == DocumentTypeEnum.Small)
            {
                if ((vehicleModel.PlateNumber != null ||
                    vehicleModel.Brand != null) &&
                    recognizedFieldsCounter < 6)
                {
                    return true;
                }
                else if (recognizedFieldsCounter < 2)
                {
                    return true;
                }else
                {
                    return false;
                }
            }
            else if (recognizedFieldsCounter < 8)
            {
                return true;
            }

            return false;
        }

        private static string AdaptLetterCase(string word)
        {
            word = word[0] + word.Substring(1, word.Length - 1).ToLower();
            return word;
        }

        private static string LatinToCyrillicLetters(string carPlate)
        {
            if (carPlate.Contains('С'))
            {
                carPlate = carPlate.Replace('С', 'C');
            }

            if (carPlate.Contains('Р'))
            {
                carPlate = carPlate.Replace('Р', 'P');
            }

            if (carPlate.Contains('Х'))
            {
                carPlate = carPlate.Replace('Х', 'X');
            }

            if (carPlate.Contains('М'))
            {
                carPlate = carPlate.Replace('М', 'M');
            }

            if (carPlate.Contains('Н'))
            {
                carPlate = carPlate.Replace('Н', 'H');
            }

            if (carPlate.Contains('К'))
            {
                carPlate = carPlate.Replace('К', 'K');
            }

            if (carPlate.Contains('Т'))
            {
                carPlate = carPlate.Replace('Т', 'T');
            }

            if (carPlate.Contains('О'))
            {
                carPlate = carPlate.Replace('О', 'O');
            }

            if (carPlate.Contains('В'))
            {
                carPlate = carPlate.Replace('В', 'B');
            }

            return carPlate;
        }
        #endregion
    }
}
