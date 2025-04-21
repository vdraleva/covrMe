using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Shared.Constants
{
    public class Mutations
    {
        #region Users

        public const string RegisterMutation = "mutation ($registerUserInput:RegisterUserInput!) {\r\n  registerUser(input:  $registerUserInput){\r\n    userId,\r\n      code,\r\n      message\r\n  }     \r\n}";
        public const string LoginMutation = "mutation ($loginInput:LoginInput!) {\r\n  login(input:  $loginInput){\r\n    jwt,\r\n      code,\r\n      message,\r\n      user{\r\n        email,\r\n        id,\r\n        drivingExperience,\r\n        phoneNumber,\r\n        firstName,\r\n        surName,\r\n        lastName,\r\n        latinFirstName,\r\n        latinSurName,\r\n        latinLastName,\r\n        uiNumber,\r\n        vatNumber,\r\n        address,\r\n        birthDate,\r\n        address,\r\n        regionId,\r\n        municipalityId,\r\n        cityId,\r\n        countryId,\r\n        postCode\r\n        roles,\r\n        latinAddress,\r\n        latinCompanyName\r\n    }\r\n  }     \r\n}";
        public const string SendEmailForgottenPassword = "mutation ($sendEmailInput:SendEmailForgottenPasswordInput!) \r\n{ \r\n  sendEmailForgottenPassword(input:  $sendEmailInput)\r\n  {\r\n    code\r\n    message\r\n  }    \r\n}";
        public const string ResetUserPassword = "mutation ($resetUserPasswordInput:ResetUserPasswordInput!) {\r\n  resetUserPassword(input:  $resetUserPasswordInput){\r\n    code\r\n    message\r\n  }     \r\n}";
        public const string AuthenticateMutation = "mutation ($authenticateInput:AuthenticateInput!) {\r\n  authenticate(input:  $authenticateInput){\r\n      user{\r\n        firstName,\r\n        lastName,\r\n        email,\r\n        id,\r\n        phoneNumber\r\n    }\r\n  }     \r\n}";
        public const string EditUserInfoMutation = "mutation ($editUserInfoInput:EditUserInfoInput!) {\r\n  editUserInfo(input:  $editUserInfoInput){\r\n      user{\r\n        email,\r\n        id,\r\n        drivingExperience,\r\n        phoneNumber,\r\n        firstName,\r\n        surName,\r\n        lastName,\r\n        latinFirstName,\r\n        latinSurName,\r\n        latinLastName,\r\n        uiNumber,\r\n        vatNumber,\r\n        address,\r\n        birthDate,\r\n        address,\r\n        regionId,\r\n        municipalityId,\r\n        cityId,\r\n        countryId,\r\n        postCode\r\n        message,\r\n        latinAddress,\r\n        latinCompanyName\r\n   companyName   }   \r\n  }     \r\n}";
        public const string ChangeUserPassword = "mutation ($changeUserPasswordInput:ChangeUserPasswordInput!) { changeUserPassword(input:  $changeUserPasswordInput){\r\n       code\r\n    }\r\n}";
        public const string AddUserToFamilyAndFriends = "mutation ($addUserToFamilyAndFriendsInput:AddUserToFamilyAndFriendsInput!) {\r\n  addUserToFamilyAndFriends(input:  $addUserToFamilyAndFriendsInput){\r\n      user{\r\n        email,\r\n        id,\r\n        drivingExperience,\r\n        phoneNumber,\r\n        firstName,\r\n        surName,\r\n        lastName,\r\n        latinFirstName,\r\n        latinSurName,\r\n        latinLastName,\r\n        uiNumber,\r\n        vatNumber,\r\n        address,\r\n        birthDate,\r\n        address,\r\n        regionId,\r\n        municipalityId,\r\n        cityId,\r\n        countryId,\r\n        postCode\r\n        latinAddress,\r\n        latinCompanyName\r\n  companyName    }\r\n  }     \r\n}";
        public const string DeleteUser = "mutation ($deleteUserInput:DeleteUserInput!) {\r\n  deleteUser(input:  $deleteUserInput){\r\n    code\r\n  }     \r\n}";
        public const string AddMultipleUsersToFamilyAndFriends = "mutation ($addMultipleUserToFamilyAndFriendsInput:AddMultipleUserToFamilyAndFriendsInput!) {\r\n  addMultipleUserToFamilyAndFriends(input:  $addMultipleUserToFamilyAndFriendsInput){\r\n    code\r\n  }     \r\n}";
        public const string EditMultipleFamilyAndFriendsUser = "mutation ($editMultipleFamilyFriendsUserInput:EditMultipleFamilyFriendsUserInput!) {\r\n  editMultipleFamilyFriendsUser(input:  $editMultipleFamilyFriendsUserInput){\r\n    code\r\n  }     \r\n}";
        public const string GetUserGuiltTypes = "mutation {\r\n  userGuiltTypes {\r\n    getUserGuiltTypesPayload{\r\n      guiltTypes\r\n    }\r\n  }\r\n}";
        public const string CheckResetPasswordCode = "mutation ($checkResetPasswordCodeInput:CheckResetPasswordCodeInput!) \r\n{ \r\n  checkResetPasswordCode(input:  $checkResetPasswordCodeInput)\r\n  {\r\n    code\r\n    message\r\n    isCodeValid\r\n  }    \r\n}";
        #endregion

        #region Vehicles

        public const string AddVehicleMutation = "mutation ($addVehicleInput:AddVehicleInput!) {\r\n  addVehicle(input:  $addVehicleInput){\r\n    vehicle{\r\n      id,\r\n      registrationCertificateNumber,\r\n      plateNumber,\r\n      userId,\r\n      modelId,\r\n      model,\r\n      brand,\r\n      brandId,\r\n      vehicleType,\r\n      vehicleTypeId,\r\n      vehicleUsage,\r\n      vehicleUsageId,\r\n      firstRegistrationDate,\r\n      vin,\r\n      bodyType,\r\n      places,\r\n      color,\r\n      engineType,\r\n      vehicleKilowatts,\r\n      steeringWheel,\r\n      engineVolume,\r\n      netWeight,\r\n      grossWeight,\r\n      message,\r\n      exceptionType\r\n    }\r\n  }\r\n}     ";
        public const string EditVehicleMutation = "mutation ($editVehicleInput:EditVehicleInput!) {\r\n  editVehicle(input:  $editVehicleInput){\r\n    vehicle{\r\n      id,\r\n      registrationCertificateNumber,\r\n      plateNumber,\r\n      userId,\r\n      modelId,\r\n      model,\r\n      brand,\r\n      brandId,\r\n      vehicleType,\r\n      vehicleTypeId,\r\n      vehicleUsage,\r\n      vehicleUsageId,\r\n      firstRegistrationDate,\r\n      vin,\r\n      bodyType,\r\n      places,\r\n      color,\r\n      engineType,\r\n      vehicleKilowatts,\r\n      steeringWheel,\r\n      engineVolume,\r\n      netWeight,\r\n      grossWeight,\r\n      message,\r\n      exceptionType\r\n    }\r\n  }\r\n}   ";
        public const string GetVehicleBodyTypesMutation = "mutation {\r\n  vehicleBodyTypes {\r\n    getVehicleBodyTypesPayload{\r\n      bodyTypes\r\n    }\r\n  }\r\n}";
        public const string GetVehicleColorsMutation = "mutation {\r\n  vehicleColors {\r\n    getVehicleColorsPayload{\r\n      colors\r\n    }\r\n  }\r\n}";
        public const string GetVehiclesEngineTypes = "mutation {\r\n  vehicleEngineTypes {\r\n    getVehicleEngineTypesPayload{\r\n      engineTypes\r\n    }\r\n  }\r\n}";
        public const string GetVehicleModelsMutation = "mutation($vehicleModelsInput:VehicleModelsInput!) {\r\n   vehicleModels(input:  $vehicleModelsInput){\r\n    models\r\n  }\r\n}";
        #endregion

        #region Civil Insurance

        public const string CivilInsuranceSearch = "mutation ($civilInsuranceSearchInput:CivilInsuranceSearchInput!) {\r\n  civilInsuranceSearch(input:  $civilInsuranceSearchInput){\r\n    insuranceOffers{\r\n      success,\r\n      insuranceCompanyName,\r\n      civilInsuranceOffer{\r\n        civilInsuranceCalculation{\r\n          premiumWithTax,         \r\n          installments{\r\n            firstInstallment{\r\n              premiumWithTax,\r\n              paymentDate      \r\n            },\r\n            secondInstallment{\r\n              premiumWithTax,\r\n              paymentDate\r\n            },\r\n            thirdInstallment{\r\n              premiumWithTax,\r\n              paymentDate\r\n            },\r\n            fourthInstallment{\r\n              premiumWithTax,\r\n              paymentDate\r\n            }\r\n          },\r\n        fullInstallmentsBreakdown{\r\n            firstInstallment{\r\n                firstInstallment{\r\n                  premiumWithTax,\r\n                  paymentDate      \r\n                }\r\n            },\r\n            secondInstallment{\r\n                firstInstallment{\r\n                    premiumWithTax,\r\n                    paymentDate      \r\n                },\r\n                secondInstallment{\r\n                    premiumWithTax,\r\n                    paymentDate\r\n                }\r\n            },\r\n            fourthInstallment{\r\n              firstInstallment{\r\n                premiumWithTax,\r\n                paymentDate      \r\n              },\r\n              secondInstallment{\r\n                premiumWithTax,\r\n                paymentDate\r\n              },\r\n              thirdInstallment{\r\n                premiumWithTax,\r\n                paymentDate\r\n              },\r\n              fourthInstallment{\r\n                premiumWithTax,\r\n                paymentDate\r\n              }\r\n            }\r\n          }\r\n        }\r\n      } \r\n    }\r\n  }     \r\n}";
        
        public const string UpdateCivilInsuranceDocs = "mutation ($updateCivilInsuranceDocumentStatusInput:UpdateCivilInsuranceDocumentStatusInput!) {\r\n  updateCivilInsuranceDocumentStatus(input:  $updateCivilInsuranceDocumentStatusInput){\r\n    isUpdated\r\n  } \r\n}";
        public const string CivilInsurancePolicy = "mutation ($civilInsurancePolicyInput:CivilInsurancePolicyInput!) {\r\n  civilInsurancePolicy(input:  $civilInsurancePolicyInput){\r\n    success,\r\n    errorId,\r\n    policyNo,\r\n    message\r\n  }     \r\n}";
        public const string CheckVehicleCivilInsuranceAllowed = "mutation ($checkVehicleCivilInsuranceAllowedInput:CheckVehicleCivilInsuranceAllowedInput!) {\r\n  checkVehicleCivilInsuranceAllowed(input:  $checkVehicleCivilInsuranceAllowedInput){\r\n      isForbidden,\r\n      endDate\r\n  }     \r\n}";
        public const string CivilInsuranceInstallment = "mutation ($civilInsuranceInstallmentInput:CivilInsuranceInstallmentInput!) {\r\n  civilInsuranceInstallment(input:  $civilInsuranceInstallmentInput){\r\n    code,\r\n    message,\r\n    errorId\r\n  }     \r\n}";
        public const string CivilInsuranceLongSearch = "mutation ($civilInsuranceLongSearchInput:CivilInsuranceLongSearchInput!) {\r\n  civilInsuranceLongSearch(input:  $civilInsuranceLongSearchInput){\r\n    insuranceOffers{\r\n      success,\r\n      insuranceCompanyName,\r\n      civilInsuranceOffer{\r\n        civilInsuranceCalculation{\r\n          premiumWithTax,         \r\n          installments{\r\n            firstInstallment{\r\n              premiumWithTax,\r\n              paymentDate      \r\n            },\r\n            secondInstallment{\r\n              premiumWithTax,\r\n              paymentDate\r\n            },\r\n            thirdInstallment{\r\n              premiumWithTax,\r\n              paymentDate\r\n            },\r\n            fourthInstallment{\r\n              premiumWithTax,\r\n              paymentDate\r\n            }\r\n          },\r\n          fullInstallmentsBreakdown{\r\n            firstInstallment{\r\n                firstInstallment{\r\n                  premiumWithTax,\r\n                  paymentDate      \r\n                }\r\n            },\r\n            secondInstallment{\r\n                firstInstallment{\r\n                    premiumWithTax,\r\n                    paymentDate      \r\n                },\r\n                secondInstallment{\r\n                    premiumWithTax,\r\n                    paymentDate\r\n                }\r\n            },\r\n            fourthInstallment{\r\n              firstInstallment{\r\n                premiumWithTax,\r\n                paymentDate      \r\n              },\r\n              secondInstallment{\r\n                premiumWithTax,\r\n                paymentDate\r\n              },\r\n              thirdInstallment{\r\n                premiumWithTax,\r\n                paymentDate\r\n              },\r\n              fourthInstallment{\r\n                premiumWithTax,\r\n                paymentDate\r\n              }\r\n            }\r\n          }\r\n        }\r\n      } \r\n    }\r\n  }     \r\n}";
        public const string CivilInsuranceLongPolicy = "mutation ($civilInsuranceLongPolicyInput:CivilInsuranceLongPolicyInput!) {\r\n  civilInsuranceLongPolicy(input:  $civilInsuranceLongPolicyInput){\r\n    success,\r\n    errorId,\r\n    policyNo,\r\n    message\r\n  }     \r\n}";

        #endregion

        #region Travel Insurance

        public const string TravelCalculation = "mutation ($travelCalculationInput:TravelCalculationInput!) {\r\n  travelCalculation(input:  $travelCalculationInput){\r\n    calculationResult{\r\n      id,\r\n      premiumWithoutTax,\r\n      premium,\r\n      companyName,\r\n      tax,\r\n      errorId\r\n    }\r\n  }     \r\n}";
        public const string TravelPolicy = "mutation ($travelPolicyInput:TravelPolicyInput!) {\r\n  travelPolicy(input:  $travelPolicyInput){\r\n    policyNo,\r\n    errorId,\r\n    success,\r\n    message\r\n  }     \r\n}";



        #endregion

        #region Mountain Insurance

        public const string MountainCalculation = "mutation ($mountainCalculationInput:MountainCalculationInput!) {\r\n  mountainCalculation(input:  $mountainCalculationInput){\r\n    calculationResult{\r\n      id,\r\n      premiumWithoutTax,\r\n      premium,\r\n      companyName,\r\n      tax,\r\n      errorId\r\n    }\r\n  }     \r\n}";
        public const string MountainPolicy = "mutation ($mountainPolicyInput:MountainPolicyInput!) {\r\n  mountainPolicy(input:  $mountainPolicyInput){\r\n    policyNo,\r\n    errorId,\r\n    success,\r\n    message\r\n  }     \r\n}";

        #endregion

        #region Health Insurance

        public const string HealthCalculation = "mutation ($healthCalculationInput:HealthCalculationInput!) {\r\n  healthCalculation(input:  $healthCalculationInput){\r\n    calculationResult{\r\n      id,\r\n      premiumWithoutTax,\r\n      premium,\r\n      companyName,\r\n      tax,\r\n      errorId\r\n    }\r\n  }     \r\n}";
        public const string HealthPolicy = "mutation ($healthPolicyInput:HealthPolicyInput!) {\r\n  healthPolicy(input:  $healthPolicyInput){\r\n    policyNo,\r\n    errorId,\r\n    success,\r\n    message\r\n  }     \r\n}";
        public const string HealthInstallment = "mutation ($healthInsuranceInstallmentInput:HealthInsuranceInstallmentInput!) {\r\n  healthInsuranceInstallment(input:  $healthInsuranceInstallmentInput){\r\n    code  \r\n  }     \r\n}";
        #endregion

        #region MyThings Insurance

        public const string MyThingsCalculation = "mutation ($myThingsCalculationInput:MyThingsCalculationInput!) {\r\n  myThingsCalculation(input:  $myThingsCalculationInput){\r\n    calculationResult{\r\n      id,\r\n      premiumWithoutTax,\r\n      premium,\r\n      companyName,\r\n      tax,\r\n      errorId\r\n    }\r\n  }     \r\n}";
        public const string MyThingsPolicy = "mutation ($myThingsPolicyInput:MyThingsPolicyInput!) {\r\n  myThingsPolicy(input:  $myThingsPolicyInput){\r\n    policyNo,\r\n    errorId,\r\n    success,\r\n    message\r\n  }     \r\n}";

        #endregion

        #region Casco Insurance
        public const string SendEmailCascoMutation = "mutation ($sendEmailCascoInput:SendEmailCascoRequestInput!) {\r\n    sendEmailCascoRequest(input: $sendEmailCascoInput){ \r\n     code,\r\n     message\r\n     }\r\n  }";
        #endregion

        #region Speedy

        public const string SpeedyFindOffice = "mutation ($findOfficeInput:FindOfficeInput!) {\r\n  findOffice(input:  $findOfficeInput){\r\n    offices{\r\n      id,\r\n        name,\r\n        siteId\r\n    }\r\n    }\r\n  } ";
        public const string SpeedyCalculation = "mutation ($calculationInput:CalculationInput!) {\r\n  calculation(input:  $calculationInput){\r\n    calculations{\r\n        price{\r\n          amount,\r\n          vat,\r\n          total\r\n        }\r\n      } \r\n  } \r\n}";
        public const string SpeedyShipment = "mutation ($shipmentInput:ShipmentInput!) {\r\n  shipment(input:  $shipmentInput){\r\n    id,\r\n    deliveryDeadline\r\n  } \r\n}";

        #endregion

        #region Dsk

        public const string CivilInsurancePayment = "mutation ($createPaymentInput:CreatePaymentInput!) {\r\n  createPayment(input:  $createPaymentInput){\r\n     localOrderNumber,\r\n     formUrl,\r\n     documentBatchId,\r\n     stickerId,\r\n     greencardId\r\n  } \r\n}";
        public const string CheckPaymentStatus = "mutation ($checkPaymentStatusInput:CheckPaymentStatusInput!) {\r\n  checkPaymentStatus(input:  $checkPaymentStatusInput){\r\n    localOrderNumber,\r\n    status,\r\n    operation\r\n  }     \r\n}";

        #endregion

        #region UserInsurances

        public const string UserInsurancesMutation = "mutation ($userInsurancesInput:UserInsurancesInput!) {\r\n  userInsurances(input:  $userInsurancesInput){\r\n    insurances{\r\n      id,\r\n      startDate,\r\n      policyEndDate,\r\n      currentEndDate,\r\n      price,\r\n      type,\r\n      policyNo,\r\n        installmentToPay\r\n      insuranceCompany{\r\n        id,\r\n        companyName\r\n      },\r\n      vehicle{\r\n        id,\r\n        registrationCertificateNumber,\r\n        plateNumber,\r\n        brand,\r\n        model\r\n      },\r\n      civilInsurance{\r\n        id,       \r\n        secondInstallmentPrice,\r\n        thirdInstallmentPrice,\r\n        fourthInstallmentPrice,\r\n      },\r\n      healthInsurance{\r\n        id,\r\n        installmentPrice,\r\n        isFamily,\r\n        packageType\r\n      },\r\n      mountainInsurance{\r\n        id,\r\n        compensationAmount,\r\n        isExtreme\r\n      },\r\n      travelInsurance{\r\n        id,\r\n        territorialValidity,\r\n        travelPurpose,\r\n        compensationAmount\r\n      },\r\n      myThingsInsurance{\r\n        id,\r\n        propertyType,\r\n        objectType,\r\n        insuranceSum,\r\n        brand,\r\n        model\r\n      },\r\n      insuredUsers{\r\n        userId,\r\n        firstName,\r\n        lastName,\r\n        isInsurer,\r\n        isMainUser,\r\n        isUsualDriver,\r\n        birthDate,\r\n        isInsured\r\n      }\r\n    }\r\n  }     \r\n}";


        #endregion
    }
}
