namespace Application.Features.Auth.Constants;

public static class AuthMessages
{
    public const string EmailAuthenticatorDontExists = "Email authenticator don't exists.";
    public const string OtpAuthenticatorDontExists = "Otp authenticator don't exists.";
    public const string AlreadyVerifiedOtpAuthenticatorIsExists = "Already verified otp authenticator is exists.";
    public const string EmailActivationKeyDontExists = "Email Activation Key don't exists.";
    public const string UserDontExists = "Kullan�c� Bulunamad�.";
    public const string UserHaveAlreadyAAuthenticator = "User have already a authenticator.";
    public const string RefreshDontExists = "Refresh don't exists.";
    public const string InvalidRefreshToken = "Invalid refresh token.";
    public const string UserMailAlreadyExists = "Mail zaten kullan�l�yor.";
    public const string PasswordDontMatch = "�ifre E�le�miyor.";
    public const string NewPasswordShouldBeDifferent = "�ifreniz son �ifreyle any� olamaz.";
    public const string UserIsNotAStudent = "��renci de�ilsiniz buraya giri� yetkiniz bulunmuyor";
    public const string UserIsNotAAdmin = "Yetkili de�ilsiniz buraya giri� yetkiniz bulunmuyor";
    public const string OperationClaimShouldBeExist = "Herhangi bir yetkiniz bulunmuyor l�tfen yetkililere dan���n";
    public const string UserOperationClaimShouldBeExist = "Herhangi bir yetkiniz bulunmuyor l�tfen yetkililere dan���n";
}
