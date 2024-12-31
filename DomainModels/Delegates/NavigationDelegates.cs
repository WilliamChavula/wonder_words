namespace DomainModels.Delegates;

public delegate Task AuthenticationErrorDelegate();

public delegate Task QuoteSelectedDelegate(string id);

public delegate Task SignUpTapDelegate();

public delegate Task CancelTapDelegate();

public delegate Task EmailRequestSuccessDelegate();

public delegate Task SignInTapDelegate();

public delegate Task UpdateProfileTapDelegate(string email, string username);

public delegate Task SignInSuccessDelegate();

public delegate Task ForgotMyPasswordTapDelegate();

public delegate Task SignUpSuccessDelegate();

public delegate Task UpdateProfileSuccessDelegate();