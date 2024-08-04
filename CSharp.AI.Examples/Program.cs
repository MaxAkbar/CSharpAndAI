using CSharp.AI.Examples;

// Define the structure of the neural network
// 2 is the number of inputs, 9 is the number of neurons in the hidden layer, and 1 is the number of outputs
// we are using 2 for hours studied and a score of the exam
// 9 is the number of hidden layers
// 1 is the output if passing or failing the exam
int[] layers = [2, 9, 1];
// Create a new neural network with the specified layer structure and learning rate
// Purpose of the learning rate:
// The learning rate controls how much the model's weights are adjusted in response 
// to the estimated error each time the model weights are updated. It's essentially 
// the step size during optimization.
var nn = new NeuralNetwork(layers, 0.1);

// Define the training data: list of tuples containing input and expected output
var trainingData = new List<(double[] input, double[] output)>
{
    ([2, 0.7], [0]),   // Example 1: Input [2, 0.7], Expected output [0]
    ([8, 0.85], [1]),  // Example 2: Input [8, 0.85], Expected output [1]
    ([5, 0.6], [0]),   // Example 3: Input [5, 0.6], Expected output [0]
    ([10, 0.9], [1]),  // Example 4: Input [10, 0.9], Expected output [1]
};

// Set the number of training epochs
// An epoch in machine learning, particularly in neural network training, 
// refers to one complete pass through the entire training dataset. 
// During an epoch, each example in the training set is used once to update
// the model's parameters (weights and biases).
var epochs = 10000;

// Start the training loop
for (int i = 0; i < epochs; i++)
{
    // For each epoch, iterate through all training examples
    foreach (var (input, target) in trainingData)
    {
        // Train the neural network with the current example
        nn.Train(input, target);
    }

    // Every 1000 epochs, evaluate and print the network's performance
    if (i % 1000 == 0)
    {
        // Calculate Mean Squared Error (MSE) and accuracy
        var (mse, accuracy) = Evaluate(nn, trainingData);
        // Print the results, including epoch number, MSE, accuracy, and performance interpretation
        Console.WriteLine($"Epoch {i}: MSE = {mse:F4}, Accuracy = {accuracy:P2}, Performance: {InterpretMse(mse)}");
    }
}

// Evaluation function: calculates MSE and accuracy for the given data
static (double mse, double accuracy) Evaluate(NeuralNetwork nn, List<(double[] input, double[] target)> data)
{
    double totalError = 0;
    int correctPredictions = 0;

    // Iterate through all data points
    foreach (var (input, target) in data)
    {
        // Get the network's output for the current input
        var output = nn.FeedForward(input);
        // Calculate the squared error
        totalError += Math.Pow(target[0] - output[0], 2);
        // Check if the prediction is correct (rounded to nearest integer)
        if (Math.Round(output[0]) == target[0])
        {
            correctPredictions++;
        }
    }

    // Calculate Mean Squared Error
    double mse = totalError / data.Count;
    // Calculate accuracy
    double accuracy = (double)correctPredictions / data.Count;

    return (mse, accuracy);
}

// Function to interpret MSE values as performance levels
static string InterpretMse(double mse)
{
    if (mse < 0.05) return "Excellent";
    if (mse < 0.1) return "Very Good";
    if (mse < 0.2) return "Good";
    if (mse < 0.3) return "Fair";
    if (mse < 0.4) return "Poor";
    return "Very Poor";
}