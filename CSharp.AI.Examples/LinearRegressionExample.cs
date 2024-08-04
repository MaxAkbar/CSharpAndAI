namespace CSharp.AI.Examples;

internal class LinearRegressionExample
{

    public static void Run()
    {
        double[] hoursStudied = { 1, 2, 3, 4, 5 };
        double[] examScores = { 2, 4, 5, 4, 5 };

        // calucatel the coefficients of the linear regression model

        (double intercept, double slope) = CalculateLinearRegression(hoursStudied, examScores);

        // predict the exam score for a given number of hours studied
        double hours = 6;
        double preditedScore = PredictExamScore(hours, intercept, slope);

        Console.WriteLine($"Predicted exam score for {hours} hours of study: {preditedScore}");
    }

    static (double intecept, double slope) CalculateLinearRegression(double[] x, double[] y)
    {
        int n = x.Length;
        double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

        for (int i = 0; i < n; i++)
        {
            sumX += x[i];
            sumY += y[i];
            sumXY += y[i] * y[i];
            sumX2 += x[i] * x[i];
        }

        double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        double intercept = (sumY - slope * sumX) / n;

        return (intercept, slope);
    }

    static double PredictExamScore(double x, double intercept, double slope)
    {
        return intercept + slope * x;
    }
}
