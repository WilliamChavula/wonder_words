namespace FormFields.lib;

/// <summary>
/// A FormZInput represents the value of a single form input field.
/// It contains information about the value as well as validity.
///
/// FormZInput should be extended to define custom FormZInput instances.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TError"></typeparam>
public abstract record FormZInput<T, TError>(T Value, bool IsPure = true)
{
    public readonly T Value = Value;

    /// <summary>
    /// If the FormZInput is pure (has not been touched/modified).
    /// Typically, when the `FormZInput` is initially created,
    /// it is created using the `FormZInput.pure` factory method to
    /// signify that the user has not modified it.
    ///
    /// For subsequent changes (in response to user input), the
    /// `FormZInput.Dirty` factory method should be used to signify that
    /// the `FormZInput` has been manipulated.
    /// </summary>
    public readonly bool IsPure = IsPure;

    /// <summary>
    /// Whether the FormZInput value is valid according to the
    /// overridden `validator`.
    ///
    /// Returns `true` if `validator` returns `null` for the
    /// current FormZInput value and `false` otherwise.
    /// </summary>
    public bool IsValid => Validator(Value) is null;


    /// <summary>
    /// Whether the FormZInput value is not valid
    /// </summary>
    public bool IsNotValid => !IsValid;


    /// <summary>
    /// Return a Validation Error if the FormZInput value is not valid
    /// </summary>
    public TError? Error => Validator(Value);

    public TError? DisplayError => Error;

    /// <summary>
    /// A function that must return a validation error if the provided
    /// value is invalid and `null` otherwise.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>TError</returns>
    protected abstract TError? Validator(T value);

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, IsPure);
    }

    public override string ToString()
    {
        return IsPure
            ? $"FormZInput<T, TError>.Pure(Value = {Value}, IsValid = {IsValid}, Error = {Error})"
            : $"FormZInput<T, TError>.Dirty((Value = {Value}, IsValid = {IsValid}, Error = {Error})";
    }
}

public enum FormZSubmissionStatus
{
    /// <summary>
    /// The form has not yet been submitted
    /// </summary>
    Initial,

    /// <summary>
    /// The form is in the process of being submitted.
    /// </summary>
    InProgress,

    /// <summary>
    /// The form has been submitted successfully.
    /// </summary>
    Success,

    /// <summary>
    /// The form submission failed.
    /// </summary>
    Failure,

    /// <summary>
    /// The form submission has been canceled.
    /// </summary>
    Canceled
}

public static class FormZSubmissionStatusExtenstion
{
    /// <summary>
    /// Indicates whether the form has not yet been submitted.
    /// </summary>
    /// <param name="status"></param>
    /// <returns>boolean</returns>
    public static bool IsInitial(this FormZSubmissionStatus status) => status == FormZSubmissionStatus.Initial;

    /// <summary>
    /// Indicates whether the form is in the process of being submitted.
    /// </summary>
    /// <param name="status"></param>
    /// <returns>boolean</returns>
    public static bool IsInProgress(this FormZSubmissionStatus status) => status == FormZSubmissionStatus.InProgress;

    /// <summary>
    /// Indicates whether the form has been submitted successfully.
    /// </summary>
    /// <param name="status"></param>
    /// <returns>boolean</returns>
    public static bool IsSuccess(this FormZSubmissionStatus status) => status == FormZSubmissionStatus.Success;

    /// <summary>
    /// Indicates whether the form submission failed.
    /// </summary>
    /// <param name="status"></param>
    /// <returns>boolean</returns>
    public static bool IsFailure(this FormZSubmissionStatus status) => status == FormZSubmissionStatus.Failure;

    /// <summary>
    /// Indicates whether the form submission has been canceled.
    /// </summary>
    /// <param name="status"></param>
    /// <returns>boolean</returns>
    public static bool IsCanceled(this FormZSubmissionStatus status) => status == FormZSubmissionStatus.Canceled;

    /// <summary>
    /// Indicates whether the form is either in progress or has been submitted
    /// successfully.
    ///
    /// This is useful for showing a loading indicator or disabling the submit
    /// button to prevent duplicate submissions.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static bool IsInProgressOrSuccess(this FormZSubmissionStatus status) =>
        status.IsSuccess() || status.IsInProgress();
}

public record FormZ
{
    /// <summary>
    /// Given an IEnumerable of FormZInputs, checks whether all inputs are valid
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns>bool</returns>
    public static bool Validate(IEnumerable<FormZInput<dynamic, dynamic>> inputs)
    {
        return inputs.All(input => input.IsValid);
    }

    /// <summary>
    /// Given an IEnumerable of FormZInputs, checks whether all inputs are pure
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns>bool</returns>
    public static bool IsPure(IEnumerable<FormZInput<dynamic, dynamic>> inputs)
    {
        return inputs.All(input => input.IsPure);
    }
}

public abstract record FormZMixin
{
    public bool IsValid(IEnumerable<FormZInput<dynamic, dynamic>> inputs)
    {
        return FormZ.Validate(inputs);
    }

    public bool IsNotValid(IEnumerable<FormZInput<dynamic, dynamic>> inputs)
    {
        return !FormZ.Validate(inputs);
    }

    public bool IsPure(IEnumerable<FormZInput<dynamic, dynamic>> inputs)
    {
        return FormZ.IsPure(inputs);
    }

    public bool IsDirty(IEnumerable<FormZInput<dynamic, dynamic>> inputs)
    {
        return !FormZ.IsPure(inputs);
    }

    public abstract IList<FormZInput<dynamic, dynamic>> Inputs { get; }
}