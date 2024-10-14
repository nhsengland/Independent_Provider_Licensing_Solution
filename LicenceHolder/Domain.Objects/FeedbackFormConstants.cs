namespace Domain.Objects;

public record FeedbackFormConstants
{
    public static string SessionFeedbackTypeId => "SessionFeedbackTypeId";
    public static string SessionFeedbackSatisfaction => "SessionFeedbackSatisfaction";
    public static string SessionFeedbackHowToImprove => "SessionFeedbackHowToImprove";
    public static string SessionFeedbackValidationFailures => "SessionFeedbackValidationFailures";
}