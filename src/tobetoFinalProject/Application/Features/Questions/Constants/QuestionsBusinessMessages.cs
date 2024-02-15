namespace Application.Features.Questions.Constants;

public static class QuestionsBusinessMessages
{
    public const string QuestionNotExists = "Question not exists.";
    public const string QuestionOptionsLessThanSeven = "7'den fazla se�enek ekleyemezsiniz";
    public const string QuestionOptionsMustBeDifferent = "Ayn� Se�ene�i 1 den fazla kez ayn� soruya ekleyemezsiniz";
    public const string PoolQuestionsMustBeDifferent = "Ayn� Havuza Ayn� Soruyu 2 veya daha fazla kere atamazs�n�z";
    public const string? QuestionOptionsHaveToContainCorrectOption = "Do�ru se�enek sorunun t�m se�eneklerinin i�erisinde yer almal�";
}