using System;

namespace CaseBusiness.ISO
{
	#region Configurations

	public enum Mastercard
    {
        BIN = 536518,
        ICA = 11620,
        ServiceCode = 601
    }

	public enum CardProgram
	{
		MCS
	}

    public enum CountryCode
    {
        Brasil = 076
    }

    public enum CurrencyCode
    {
        Real = 986
    }

    public enum EncodingType
    {
        NotDefined = 0,
        ASCII = 1,
        EBCDIC = 2
    }

	public enum ParseMode
	{
		onDemand = 0,
		Complete = 1
	}

	public enum AmountType
	{
		Ledger_Balance = 1,  
		AvailableBalance = 2,
		AmountOwing = 3,
		AmountDue = 4,
		HealthcareEligibilityAmount = 10,
		PrescriptionEligibilityAmount = 11,
		ReservedFutureUse1 = 12,
		ReservedFutureUse2 = 13,
		ReservedFutureUse3 = 14,
		AmountCashBack = 40,
		OriginalAmount = 57
	}

    public enum CardholderTransactionTypeCode
    {
        Purchase = 0,
        Withdrawal = 1,
        DebitAdjustment = 2,
        PurchaseWithCashBack = 9,
        VisaOnly = 10,
        CashDisbursement = 17,
        ScripIssue = 18,
        PurchaseReturn_Refund = 20,
        Deposit = 21,
        CreditAdjustment = 22,
        CheckDepositGuarantee = 23,
        CheckDeposit = 24,
        PaymentTransaction = 28,
        BalanceInquiry = 30,
        AccountTransfer = 40,
        ReservedForFutureUse = 90,
        PINUnblock = 91,
        PINChange = 92
    }

    public enum CardholderFromAccountTypeCode
    {
        DefaultAccount = 0,
        SavingsAccount = 10,
        CheckingAccount = 20,
        CreditCardAccount = 30,
        CreditLineAccount = 38,
        Corporate = 39,
        UniversalAccount = 40,
        MoneyMarketInvestmentAccount = 50,
        StoredValueAccount = 60,
        RevolvingLoanAccount = 90
    }

    public enum CardholderToAccountTypeCode
    {
        DefaultAccount = 0,
        SavingsAccount = 10,
        CheckingAccount = 20,
        CreditCardAccount = 30,
        CreditLineAccount = 38,
        UniversalAccount = 40,
        MoneyMarketInvestmentAccount = 50,
        IRAInvestmentAccount = 58,
        RevolvingLoanAccount = 90,
        InstallmentLoanAccount = 91,
        RealEstateLoanAccount = 92
    }

    public enum POSCardholderPresence
    {
        CardholderPresent = 0,
        CardholderNotPresent_Unspecified = 1,
        Mail_FacsimileOrder = 2,
        Phone_ARUOrder = 3,
        StandingOrder_RecurringTransactions = 4,
        ElectronicOrder = 5 // (home PC, Internet, mobile phone, PDA)
    }

    public enum POSCardCaptureCapabilities
    {
        Terminal_OperatorHasNoCardCaptureCapability = 0,
        Terminal_OperatorHasCardCaptureCapability = 1
    }

    public enum POSTransactionStatus
    {
        NormalRequest = 0,  // (original presentment)
        SecureCodePhoneOrder = 2,
        PreauthorizedRequest = 4
    }

    public enum POSCardPresence
    {
        CardPresent = 0,
        CardNotPresent = 1
    }

    public enum CardholderActivatedTerminalLevel
    {
        NotACATTransaction = 0,
        AuthorizedLevel1CAT = 1,    // Automated dispensing machine with PIN
        AuthorizedLevel2CAT = 2,    // Self-service terminal
        AuthorizedLevel3CAT = 3,    // Limited-amount terminal
        AuthorizedLevel4CAT = 4,    // In-flight commerce
        Reserved = 5,
        AuthorizedLevel6CAT = 6,    // Electronic commerce
        AuthorizedLevel7CAT = 7     // Transponder transaction
    }

    public enum POSCardDataTerminalInputCapabilityIndicator
    {
        UnknownOrUnspecified = 0,
        NoTerminalUsed = 1,                                                         // (voice/ARU authorization)
        MagneticStripeReaderOnly = 2,
        ContactlessM_Chip = 3,                                                      // (Proximity Chip). Terminal supports PayPass M/Chip and PayPass magstripe transactions. The terminal also may support contact transactions, however this value must only be used for contactless transactions.
        ContactlessMagneticStripe = 4,                                              // (Proximity Chip) only. Terminal supports PayPass magstripe transactions. The terminal also may support contact transactions, however this value must only be used for contactless magstripe transactions.
        EMVSpecification_CompatibleChipReader_AndMagneticStripeReader = 5,          // The terminal also may support contactless transactions, however these values must only be used for contact transactions.
        KeyEntryOnly = 6,
        MagneticStripeReaderAndKeyEntry = 7,
        EMVSpecification_CompatibleChipReader_MagneticStripeReaderAndKeyEntry = 8,  // The terminal also may support contactless transactions; however, these values must only be used for contact transactions.
        EMVSpecification_CompatibleChipReader_Only = 9                              // The terminal also may support contactless transactions; however, this value only must be used for contact transactions.
    }

    public enum POSPANEntryMode
    {
        PANEntryModeUnknown = 0,
        PANManualEntry = 1,
        PANAuto_entryViaMagneticStripeTrackDataIsNotRequired = 2,
        PANAuto_entryViaBarCodeReader = 3,
        PANAuto_entryViaOpticalCharacterReader_OCR = 4,
        PANAuto_entryViaChip = 5,
        PANAuto_entryViaChipPayPassMappingServiceApplied = 6,
        PANAuto_entryViaContactlessM_Chip = 7,
        PANAuto_entryViaContactlessM_ChipPayPassMappingServiceApplied = 8,
        AHybridTerminalWithAnOnlineConnectionToTheAcquirerFailedInSendingAChipFallbackTransaction = 79,
        ChipCardAtChip_capableTerminalWasUnableToProcessTransactionUsingDataOnTheChip = 80,
        PANEntryViaElectronicCommerce_IncludingChip = 81,
        PANAuto_entryViaMagneticStripe = 90,
        PANAuto_entryViaContactlessMagneticStripe = 91,
        ContactlessInput = 92,
        VisaOnly = 95
    }

    public enum POSPINEntryMode
    {
        UnspecifiedOrUnknown = 0,
        TerminalHasPINEntryCapability = 1,
        TerminalDoesNotHavePINEntryCapability = 2,
        TerminalHasPINEntryCapabilityButPINPadIsNotCurrentlyOperative = 8
    }

    public enum ParcelasPlanType
    {
        IssuerFinanced = 20,
        MerchantFinanced = 21
    }

    public enum TipoCapturaCartao
    {
        Nenhum = 0,
        Chip = 1,
        Manual = 2,
        Trilha = 3
    }

    public enum ModalidadeTransacao
    {
        Nenhum = 0,
        AVista = 1,
        ParceladoLojista = 2,
        ParceladoEmissor = 3,
        PreAutorizacao = 4
    }

    public enum MTI
    {
        // Family: Authorization/01xx Messages
        AuthorizationRequest = 0x0100,
        AuthorizationRequestResponse = 0x0110,
        AuthorizationAdvice = 0x0120,
        AuthorizationAdviceResponse = 0x0130,
        AuthorizationAcknowledgement = 0x0180,
        AuthorizationResponseNegativeAcknowledgement = 0x0190,

        // Family: Issuer File Update/03xx Messages
        IssuerFileUpdateRequest = 0x0302,
        IssuerFileUpdateRequestResponse = 0x0312,

        // Family: Reversal/04xx Messages
        ReversalRequest = 0x0400,
        ReversalRequestResponse = 0x0410,
        ReversalAdvice = 0x0420,
        ReversalAdviceResponse = 0x0430,

        // Family: Administrative/06xx Messages
        AdministrativeRequest = 0x0600,
        AdministrativeRequestResponse = 0x0610,
        AdministrativeAdvice = 0x0620,
        AdministrativeAdviceResponse = 0x0630,

        // Family: Network Management/08xx Messages
        NetworkManagementRequest = 0x0800,
        NetworkManagementRequestResponse = 0x0810,
        NetworkManagementAdvice = 0x0820
	}

	public enum SentidoMensagem
	{
		Entrada = 1,
		Saida = 2
	}
	
    #endregion Configurations

	#region Fields/SubDatas

	public enum DataElement
    {
        d002_PrimaryAccountNumber = 2,
        d003_ProcessingCode = 3,
        d004_AmountTransaction = 4,
        d005_AmountSettlement = 5,
        d006_AmountCardholderBilling = 6,
        d007_TransmissionDateAndTime = 7,
        d008_AmountCardholderBillingFee = 8,
        d009_ConversionRateSettlement = 9,
        d010_ConversionRateCardholderBilling = 10,
        d011_SystemsTraceAuditNumber = 11,
        d012_TimeLocalTransaction = 12,
        d013_DateLocalTransaction = 13,
        d014_DateExpiration = 14,
        d015_DateSettlement = 15,
        d016_DateConversion = 16,
        d017_DateCapture = 17,
        d018_MerchantType = 18,
        d019_AcquiringInstitutionCountryCode = 19,
        d020_PrimaryAccountNumberCountryCode = 20,
        d021_ForwardingInstitutionCountryCode = 21,
        d022_POSEntryMode = 22,
        d023_CardSequenceNumber = 23,
        d024_NetworkInternationalID = 24,
        d025_POSConditionCode = 25,
        d026_POSPersonalIDNumberCaptureCode = 26,
        d027_AuthorizationIDResponseLength = 27,
        d028_AmountTransactionFee = 28,
        d029_AmountSettlementFee = 29,
        d030_AmountTransactionProcessingFee = 30,
        d031_AmountSettlementProcessingFee = 31,
        d032_AcquiringInstitutionIDCode = 32,
        d033_ForwardingInstitutionIDCode = 33,
        d034_PrimaryAccountNumberExtended = 34,
        d035_Track2Data = 35,
        d036_Track3Data = 36,
        d037_RetrievalReferenceNumber = 37,
        d038_AuthorizationIDResponse = 38,
        d039_ResponseCode = 39,
        d040_ServiceRestrictionCode = 40,
        d041_CardAcceptorTerminalID = 41,
        d042_CardAcceptorIDCode = 42,
        d043_CardAcceptorNameAndLocation = 43,
        d044_AdditionalResponseData = 44,
        d045_Track1Data = 45,
        d046_AdditionalDataISOUse = 46,
        d047_AdditionalDataNationalUse1 = 47,
        d048_AdditionalDataPrivateUse = 48,
        d049_CurrencyCodeTransaction = 49,
        d050_CurrencyCodeSettlement = 50,
        d051_CurrencyCodeCardholderBilling = 51,
        d052_PINData = 52,
        d053_SecurityRelatedControlInformation = 53,
        d054_AdditionalAmounts = 54,
        d055_IntegratedCircuitCardSystemRelatedData = 55,
        d056_ReservedforISOUse = 56,
        d057_ReservedforNationalUse1 = 57,
        d058_ReservedforNationalUse2 = 58,
        d059_ReservedforNationalUse3 = 59,
        d060_AdviceReasonCode = 60,
        d061_POSData = 61,
        d062_IntermediateNetworkFacilityData = 62,
        d063_NetworkData = 63,
        d064_MessageAuthenticationCodePrimaryBitmap = 64,
        d065_BitMapExtended = 65,
        d066_SettlementCode = 66,
        d067_ExtendedPaymentCode = 67,
        d068_ReceivingInstitutionCountryCode = 68,
        d069_SettlementInstitutionCountryCode = 69,
        d070_NetworkManagementInformationCode = 70,
        d071_MessageNumber = 71,
        d072_MessageNumberLast = 72,
        d073_DateAction = 73,
        d074_CreditsNumber = 74,
        d075_CreditsReversalNumber = 75,
        d076_DebitsNumber = 76,
        d077_DebitsReversalNumber = 77,
        d078_TransfersNumber = 78,
        d079_TransfersReversalNumber = 79,
        d080_InquiriesNumber = 80,
        d081_AuthorizationsNumber = 81,
        d082_CreditsProcessingFeeAmount = 82,
        d083_CreditsTransactionFeeAmount = 83,
        d084_DebitsProcessingFeeAmount = 84,
        d085_DebitsTransactionFeeAmount = 85,
        d086_CreditsAmount = 86,
        d087_CreditsReversalAmount = 87,
        d088_DebitsAmount = 88,
        d089_DebitsReversalAmount = 89,
        d090_OriginalDataElements = 90,
        d091_IssuerFileUpdateCode = 91,
        d092_FileSecurityCode = 92,
        d093_ResponseIndicator = 93,
        d094_ServiceIndicator = 94,
        d095_ReplacementAmounts = 95,
        d096_MessageSecurityCode = 96,
        d097_AmountNetSettlement = 97,
        d098_Payee = 98,
        d099_SettlementInstitutionIDCode = 99,
        d100_ReceivingInstitutionIDCode = 100,
        d101_FileName = 101,
        d102_AccountID1 = 102,
        d103_AccountID2 = 103,
        d104_TransactionDescription = 104,
        d105_ReservedForISOUse1 = 105,
        d106_ReservedForISOUse2 = 106,
        d107_ReservedForISOUse3 = 107,
        d108_ReservedForISOUse4 = 108,
        d109_ReservedForISOUse5 = 109,
        d110_ReservedForISOUse6 = 110,
        d111_ReservedForISOUse7 = 111,
        d112_AdditionalDataNationalUse2 = 112,
        d113_ReservedforNationalUse4 = 113,
        d114_ReservedforNationalUse5 = 114,
        d115_ReservedforNationalUse6 = 115,
        d116_ReservedforNationalUse7 = 116,
        d117_ReservedforNationalUse8 = 117,
        d118_ReservedforNationalUse9 = 118,
        d119_ReservedforNationalUse10 = 119,
        d120_RecordData = 120,
        d121_AuthorizingAgentID = 121,
        d122_AdditionalRecordData = 122,
        d123_ReceiptFreeText = 123,
        d124_MemberDefinedData = 124,
        d125_NewPINData = 125,
        d126_PrivateData1 = 126,
        d127_PrivateData2 = 127,
        d128_MessageAuthenticationCodeSecondaryBitmap = 128
    }

    public enum SubData
    {
        d003_ProcessingCode_s001_CardHolderTransactionTypeCode = 1,
        d003_ProcessingCode_s002_CardHolderFromAccountTypeCode = 2,
        d003_ProcessingCode_s003_CardHolderToAccountTypeCode = 3,
        d007_TransmissionDateAndTime_s001_Date = 1,
        d007_TransmissionDateAndTime_s002_Time = 2,
        d022_POSEntryMode_s001_POSTerminalPanEntryMode = 1,
        d022_POSEntryMode_s002_POSTerminalPinEntryMode = 2,
        d035_Track2Data_s001_PrimaryAccountNumber = 1,
        d035_Track2Data_s002_Separator = 2,
        d035_Track2Data_s003_ExpirationDate = 3,
        d035_Track2Data_s004_ServiceCode = 4,
        d035_Track2Data_s005_PinVerificationData = 5,
        d035_Track2Data_s006_DiscretionaryData = 6,
        d035_Track2Data_s007_ReservedSpace = 7,
        d037_RetrievalReferenceNumber_s001_TransactionDateAndInitiatorDiscretionaryData = 1,
        d037_RetrievalReferenceNumber_s002_TerminalTransactionNumber = 2,
        d043_CardAcceptorNameAndLocation_s001_MerchantName = 1,
        d043_CardAcceptorNameAndLocation_s002_Space = 2,
        d043_CardAcceptorNameAndLocation_s003_MerchantsCity= 3,
        d043_CardAcceptorNameAndLocation_s004_Space = 4,
        d043_CardAcceptorNameAndLocation_s005_MerchantsState = 5,
        d045_Track1Data_s001_FormatCode = 1,
        d045_Track1Data_s002_PrimaryAccountNumber = 2,
        d045_Track1Data_s003_Separator = 3,
        d045_Track1Data_s004_CardHolderName = 4,
        d045_Track1Data_s005_Separator = 5,
        d045_Track1Data_s006_ExpirationDate = 6,
        d045_Track1Data_s007_ServiceCode = 7,
        d045_Track1Data_s008_PinVerification = 8,
        d045_Track1Data_s009_ReservedSpace = 9,
        d045_Track1Data_s010_DiscretionaryData = 10,
        d045_Track1Data_s011_ReservedSpace = 11,
        d048_AdditionalDataPrivateUse_s001_TCC = 1,
        d048_AdditionalDataPrivateUse_s010_EncryptedBINBlockKey = 10,
        d048_AdditionalDataPrivateUse_s011_KeyExchangeBlockData = 11,
        d048_AdditionalDataPrivateUse_s012_RountingIndicator = 12,
        d048_AdditionalDataPrivateUse_s020_CardHolderVerificationMethod = 20,
        d048_AdditionalDataPrivateUse_s032_MasterCardAssignedID = 32,
        d048_AdditionalDataPrivateUse_s033_PANMappingFileInformation = 33,
        d048_AdditionalDataPrivateUse_s034_DynamicCVC3ATCInformation = 34,
        d048_AdditionalDataPrivateUse_s042ElectronicCommerceIndicators = 42,
        d048_AdditionalDataPrivateUse_s071_OnBehalfService = 71,
        d048_AdditionalDataPrivateUse_s080_PINServiceCode = 80,
		d048_AdditionalDataPrivateUse_s083_AddressVerificationServiceResponse = 83,		
		d048_AdditionalDataPrivateUse_s087_CardValidationCodeResult = 87,
		d048_AdditionalDataPrivateUse_s088_MagneticStripeComplianceStatusIndicator = 88,
        d048_AdditionalDataPrivateUse_s089_MagneticStripeComplianceErrorIndicator = 89, 
		d048_AdditionalDataPrivateUse_s092_Cvc2 = 92,
        d048_AdditionalDataPrivateUse_s095_MasterCardPromotionCode = 95,
		d054_AdditionalAmounts_s001_AccountType = 1,
		d054_AdditionalAmounts_s002_AmountType = 2,
		d054_AdditionalAmounts_s003_CurrencyCode = 3,
		d054_AdditionalAmounts_s004_Amount = 4,
		d055_IntegratedCircuitCard_s9F26_ApplicationCryptogram = 0x9F26,
		d061_POSData_s001_POSTerminalAttendence = 1,
        d061_POSData_s002_Reserved = 2,
        d061_POSData_s003_POSTerminalLocation = 3,
        d061_POSData_s004_POSCardholderPresence = 4,
        d061_POSData_s005_POSCardPresence = 5,
        d061_POSData_s006_POSCardCaptureCapabilities = 6,
        d061_POSData_s007_POSTransactionStatus = 7,
        d061_POSData_s008_POSTransactionSecurity = 8,
        d061_POSData_s009_Reserved = 9,
        d061_POSData_s010_Cardholder_ActivatedTerminalLevel = 10,
        d061_POSData_s011_POSCardDataTerminalInputCapabilityIndicator = 11,
        d061_POSData_s012_POSAuthorizationLifeCycle = 12,
        d061_POSData_s013_POSCountryCode = 13,
        d061_POSData_s014_POSPostalCode = 14,
        d063_NetworkData_s001_FinancialNetworkData = 1,
        d063_NetworkData_s002_BanknetReferenceNumber = 2,
        d090_OriginalDataElements_s001_OriginalMTI = 1,
		d090_OriginalDataElements_s002_OriginalSystemTraceAuditNumber = 2,
		d090_OriginalDataElements_s003_OriginalTransmissionDateTime = 3,
		d090_OriginalDataElements_s004_OriginalAcquiringInstitutionIDCode = 4,
		d090_OriginalDataElements_s005_OriginalForwardingInstitutionIDCode = 5,
		d095_ReplacementAmounts_s001_ActualAmountTransaction = 1,
		d095_ReplacementAmounts_s002_ActualAmountSettlement = 2,
		d095_ReplacementAmounts_s003_ActualAmountCardholderBilling = 3,
		d095_ReplacementAmounts_s004_ZeroFill = 4,
		d112_AdditionalDataNationalUse2_s001_InstallmentPaymentData = 1,
        d112_AdditionalDataNationalUse2_s002_InstallmentPaymentResponseData = 2,
		d120_RecordData_s001_AVSServiceIndicator1 = 1,
		d120_RecordData_s002_AVSServiceIndicator2 = 2,
		d120_RecordData_s003_AVSServiceIndicator3 = 3,
		d120_RecordData_s004_AVSServiceIndicator4 = 4,
		d124_MemberDefinedData_s006_DiscretionaryMessageOnSalesSlipSupported = 6,
		d124_MemberDefinedData_s007_DiscretionaryMessageOnSalesSlipCode = 7,
		d124_MemberDefinedData_s008_DiscretionaryMessageOnSalesSlipContent = 8,
		d124_MemberDefinedData_s009_PhoneCompanyID = 9,
		d124_MemberDefinedData_s010_CellPhoneNumber = 10,
		d124_MemberDefinedData_s011_MessageSecurityCode = 11,
		d124_MemberDefinedData_s012_MerchantCNPJNumber = 12,
		d124_MemberDefinedData_s013_TotalAnnualEffectiveCost = 13,
	}

	#endregion Fields/SubDatas

	#region ResponseCodes/InformationCodes

	public enum ResponseCodeAuthorizationRequest
    {
        r00_ApprovedOrCompletedSuccessfully = 00,
        r01_ReferToCardIssuer = 01,
        r03_InvalidMerchant = 03,
        r04_CaptureCard = 04,                                   // Not used in Brazil
        r05_DoNotHonor = 05,
        r08_HonorWithID = 08,
        r10_PartialApproval = 10,
		r12_InvalidTransaction = 12,
		r13_InvalidAmount = 13,
		r14_InvalidCardNumber = 14,
		r15_InvalidIssuer = 15,
		r30_FormatError = 30,
		r41_LostCard = 41,
        r43_StolenCard = 43,                                    // Not used in Brazil
		r51_InsufficientFunds_OverCreditLimit = 51,
		r54_ExpiredCard = 54,
		r55_InvalidPIN = 55,
		r57_TransactionNotPermittedToIssuer_Cardholder = 57,
		r58_TransactionNotPermittedToAcquirer_Terminal = 58,
		r61_ExceedsWithdrawalAmountLimit = 61,
		r62_RestrictedCard = 62,
        SecurityViolation = 63,
        ExceedsWithdrawalCountLimit = 65,
        ResponseReceivedLate = 68,
        ContactCardIssuer = 70,
        PINNotChanged = 71,
        AllowableNumberOfPINTriesExceeded = 75,
        Invalid_NonexistentToAccountSpecified = 76,
        Invalid_NonexistentFromAccountSpecified = 77,
        Invalid_NonexistentAccountSpecified_General = 78,
        InvalidAuthorizationLifeCycle = 84,
        NotDeclined = 85,                                   // Valid for AVS only, Balance Inquiry, or SET cardholder certificate requests (Visa Only)
        PINValidationNotPossible = 86,
        PurchaseAmountOnly_NoCashBackAllowed = 87,
        CryptographicFailure = 88,
        UnacceptablePIN_TransactionDeclined_Retry = 89,
        AuthorizationSystemOrIssuerSystemInoperative = 91,
        UnableToRouteTransaction = 92,
        DuplicateTransmissionDetected = 94,
        SystemError = 96
    }

	public enum ResponseCodeAuthorizationResponse
	{
		ApprovedOrCompletedSuccessfully = 00,
		ReferToCardIssuer = 01,
		InvalidMerchant = 03,
		CaptureCard = 04,                                   // Not used in Brazil
		DoNotHonor = 05,
		HonorWithID = 08,
		PartialApproval = 10,
		InvalidTransaction = 12,
		InvalidAmount = 13,
		InvalidCardNumber = 14,
		InvalidIssuer = 15,
		FormatError = 30,
		LostCard = 41,
		StolenCard = 43,                                    // Not used in Brazil
		InsufficientFunds_OverCreditLimit = 51,
		ExpiredCard = 54,
		InvalidPIN = 55,
		TransactionNotPermittedToIssuer_Cardholder = 57,
		TransactionNotPermittedToAcquirer_Terminal = 58,
		ExceedsWithdrawalAmountLimit = 61,
		RestrictedCard = 62,
		SecurityViolation = 63,
		ExceedsWithdrawalCountLimit = 65,
		ResponseReceivedLate = 68,
		ContactCardIssuer = 70,
		PINNotChanged = 71,
		AllowableNumberOfPINTriesExceeded = 75,
		Invalid_NonexistentToAccountSpecified = 76,
		Invalid_NonexistentFromAccountSpecified = 77,
		Invalid_NonexistentAccountSpecified_General = 78,
		InvalidAuthorizationLifeCycle = 84,
		NotDeclined = 85,                                   // Valid for AVS only, Balance Inquiry, or SET cardholder certificate requests (Visa Only)
		PINValidationNotPossible = 86,
		PurchaseAmountOnly_NoCashBackAllowed = 87,
		CryptographicFailure = 88,
		UnacceptablePIN_TransactionDeclined_Retry = 89,
		AuthorizationSystemOrIssuerSystemInoperative = 91,
		UnableToRouteTransaction = 92,
		DuplicateTransmissionDetected = 94,
		SystemError = 96
	}

	public enum ResponseCodeAuthorizationAdviceRequest
	{
		ApprovedOrCompletedSuccessfully = 00,
		ReferToCardIssuer = 01,
		InvalidMerchant = 03,
		CaptureCard = 04,                                   // Not used in Brazil
		DoNotHonor = 05,
		HonorWithID = 08,
		PartialApproval = 10,
		InvalidTransaction = 12,
		InvalidAmount = 13,
		InvalidCardNumber = 14,
		InvalidIssuer = 15,
		FormatError = 30,
		LostCard = 41,
		StolenCard = 43,                                    // Not used in Brazil
		InsufficientFunds_OverCreditLimit = 51,
		ExpiredCard = 54,
		InvalidPIN = 55,
		TransactionNotPermittedToIssuer_Cardholder = 57,
		TransactionNotPermittedToAcquirer_Terminal = 58,
		ExceedsWithdrawalAmountLimit = 61,
		RestrictedCard = 62,
		SecurityViolation = 63,
		ExceedsWithdrawalCountLimit = 65,
		ResponseReceivedLate = 68,
		ContactCardIssuer = 70,
		PINNotChanged = 71,
		AllowableNumberOfPINTriesExceeded = 75,
		Invalid_NonexistentToAccountSpecified = 76,
		Invalid_NonexistentFromAccountSpecified = 77,
		Invalid_NonexistentAccountSpecified_General = 78,
		InvalidAuthorizationLifeCycle = 84,
		NotDeclined = 85,                                   // Valid for AVS only, Balance Inquiry, or SET cardholder certificate requests (Visa Only)
		PINValidationNotPossible = 86,
		PurchaseAmountOnly_NoCashBackAllowed = 87,
		CryptographicFailure = 88,
		UnacceptablePIN_TransactionDeclined_Retry = 89,
		AuthorizationSystemOrIssuerSystemInoperative = 91,
		UnableToRouteTransaction = 92,
		DuplicateTransmissionDetected = 94,
		SystemError = 96
	}
	
	public enum ResponseCodeAuthorizationAdviceResponse
	{
		ApprovedOrCompletedSuccessfully = 00,
		InvalidCardNumber = 14,
		FormatError = 30,
		DuplicateTransmissionDetected = 94,
		SystemError = 96
	}

	public enum ResponseCodeIssuerFileUpdateRequestResponse
    {
        IssuerFileUpdateActionCompletedSuccessfully = 00,
        UnableToLocateRecordOnFile = 25,
        IssuerFileUpdateFieldEditError = 27,
        FormatError = 30,
        RequestedFunctionNotSupported = 40,
        SecurityViolation = 63,
        DuplicateAdd_ActionNotPerformed = 80,
        SystemError = 96
    }

    public enum ResponseCodeReversalRequest
    {
        Error = 06,
        CustomerCancellation = 17,
        PartialReversal = 32,
        ResponseReceivedLate = 68
    }

	public enum ResponseCodeReversalResponse
	{
		ApprovedOrCompletedSuccessfully = 00,
		ReferToCardIssuer = 01,
		InvalidTransaction = 12,
		InvalidAmount = 13,
		InvalidCardNumber = 14,
		FormatError = 30,
		LostCard = 41,
		StolenCard = 43,
		TransactionNotPermittedToIssuer_Cardholder = 57,
		TransactionNotPermittedToAcquirer_Terminal = 58,
		RestrictedCard = 62,
		SecurityViolation = 63,
		AuthorizationSystemOrIssuerSystemInoperative = 91,
		UnableToRouteTransaction = 92,
		DuplicateTransmissionDetected = 94,
		SystemError = 96
	}

	public enum ResponseCodeReversalAdviceRequest
	{
		Error = 06, 
		CustomerCancellation = 17, 
		PartialReversal = 32, 
		ResponseReceivedLate = 68, 
		TimeoutAtIssuer = 82
	}

	public enum ResponseCodeReversalAdviceResponse
	{
		ApprovedOrCompletedSuccessfully = 00,
		FormatError = 30,
		DuplicateTransmissionDetected = 94,
		SystemError = 96
	}

	public enum ResponseCodeAdministrativeRequestResponse
	{
		ReceivedAndProcessedSuccessfully = 00,
		FormatError = 30,
		TransactionNotPermittedToIssuer_Cardholder = 57,
		TransactionNotPermittedToAcquirer_Terminal = 58,
		AuthorizationSystemOrIssuerSystemInoperative = 91,
		UnableToRouterTransaction = 92,
		SystemError = 96

	}
	
	public enum ResponseCodeAdministrativeAdviceResponse
	{
		ReceivedAndProcessedSuccessfully = 00,
		FormatError = 30,
		DuplicateTransmissionDetected = 94,
		SystemError = 96
	}

    public enum ResponseCodeNetworkManagementResponse
    {
        ApprovedOrCompletedSuccessfully = 00,
        FormatError = 30,
        SecurityViolation = 63,
        KeyExchangeValidationFailed = 79,
        AuthorizationSystemOrIssuerSystemInoperative = 91,
        DuplicateSAFRequest = 94,
        SystemError = 96
    }

    public enum NetworkManagementInformationCode
    {
        SignOn = 001,                           // (by prefix)
        SignOff = 002,                          // (by prefix)
        SAFSessionRequest = 060,
        GroupSignOn = 061,                      // (by MasterCard group sign-on)
        GroupSignOff = 062,                     // (by MasterCard group sign-on)
        GroupSignOnAlternateIssuerRoute = 063,
        GroupSignOffAlternateIssuerRoute = 064,
        PrefixSignOnForPrimaryRoute = 065,      // (by Group Sign-on ID for primary route)
        PrefixSignOffForPrimaryRoute = 066,     // (by Group Sign-on ID for primary route)
        PrefixSignOnForAlternateRoute = 067,    // (by Group Sign-on ID for alternate issuer route)14
        PrefixSignOffForAlternateRoute = 068,   // (by Group Sign-on ID for alternate issuer route)14
        SignOnToRiskFinderByPrefix = 070,       // (member requests RiskFinder-scored Administrative Advice/0620 messages)
        SignOffToRiskFinderByPrefix = 071,      // (does not request RiskFinder-scored Administrative Advice/0620 messages)
        MemberWantsToReceiveByPrefixRiskFinderScoredAdministrativeAdvice0620MessagesFromSAFOrEndOfFileEncounteredForRiskFinderSAFTraffic = 072,
        HostSessionActivation = 081,
        HostSessionDeactivation = 082,
        EncryptionKeyExchangeRequest = 161,
        SolicitationForKeyExchangeRequest = 162,
        NetworkConnectionStatusEchoTest = 270,
        EndOfFileEncounteredForSAFTraffic = 363
	}

	#endregion ResponseCodes/InformationCodes

	#region Status

	public enum StatusComunicacaoMIP
    {
        PendenteRespostaSignON,
        PendenteRespostaSignOFF,
        Conectado,
        Desconectado,
        FalhaNoEchoTest,
        FalhaNoSAFRequest
    }

    public enum StatusTransacao
    {
        AprovadaPeloAutorizador = 1,
        AprovadaPeloStandIN = 2,
        CanceladaPeloAutorizador = 3,
        CanceladaPeloStandIN = 4,
        CanceladaPorDecursoDePrazo = 5,
        NegadaPeloAutorizador = 6,
        NegadaPeloStandIN = 7,
        BaixaParaVenda = 8
	}

	public enum StatusBloqueio
	{
		PendenteEnvio = 1,
		SelecionadoParaEnvio = 2,  
		Enviado = 3,
		ErroEnvio = 4,
		BloqueioEfetuadoComSucesso = 5,
		BloqueioNaoEfetuado = 6,
		Cancelado = 7
	}

	#endregion Status
}