

namespace CSharp.AI.Examples;

internal class NeuralNetwork
{
    private readonly List<double[,]> _weightsList;
    private readonly int[] _layers;
    private readonly double _learningRate;

    public NeuralNetwork(int[] layerSize, double lr)
    { 
        _layers = layerSize;
        _learningRate = lr;
        _weightsList = new List<double[,]>();

        for (int i = 1; i < _layers.Length; i++)
        {
            _weightsList.Add(InitializeWeights(_layers[i], _layers[i - 1]));
        }
    }

    private double[,] InitializeWeights(int rows, int cols)
    {
        var rnd = new Random();
        var weights = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                weights[i, j] = rnd.NextDouble() - 0.5;
            }
        }

        return weights;
    }

    public void Train(double[] inputs, double[] targets)
    {
        var layOutputs = new List<double[]> { inputs };
        var currentOutputs = inputs;

        // forward pass
        foreach (var w in _weightsList)
        {
            currentOutputs = Activate(currentOutputs, w);
            layOutputs.Add(currentOutputs);
        }

        // backward pass
        var deltas = new List<double[]>();

        for (var i = _layers.Length - 1; i >= 1; i--)
        {
            if (i == _layers.Length - 1)
            {
                var outputDeltas = new double[_layers[i]];

                for (var j = 0; j < _layers[i]; j++)
                {
                    var error = targets[j] - currentOutputs[j];

                    outputDeltas[j] = error * SigmoidDerivative(currentOutputs[j]);
                }

                deltas.Add(outputDeltas);
            }
            else
            {
                var hiddenDeltas = new double[_layers[i]];

                for (var j = 0; j < _layers[i]; j++)
                {
                    double error = 0;

                    for (var k = 0; k < _layers[i + 1]; k++)
                    {
                        error += deltas[0][k] * _weightsList[i][k, j];
                    }

                    hiddenDeltas[j] = error * SigmoidDerivative(layOutputs[i][j]);
                }

                deltas.Insert(0, hiddenDeltas);
            }
        }

        // update weights
        for (var i = 0; i < _weightsList.Count; i++)
        {
            for (var j = 0; j < _weightsList[i].GetLength(0); j++)
            {
                for (var k = 0; k < _weightsList[i].GetLength(1); k++)
                {
                    _weightsList[i][j, k] += _learningRate * deltas[i][j] * layOutputs[i][k];
                }
            }
        }
    }

    private double Sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-x));
    }

    private double SigmoidDerivative(double x)
    {
        return x * (1 - x);
    }

    private double[] Activate(double[] inputs, double[,] layerWeights)
    {
        var outputs = new double[layerWeights.GetLength(0)];

        for (var i = 0; i < layerWeights.GetLength(0); i++)
        {
            double sum = 0;

            for (var j = 0; j < inputs.Length; j++)
            {
                sum += inputs[j] * layerWeights[i, j];
            }

            outputs[i] = Sigmoid(sum);
        }

        return outputs;
    }

    public double[] FeedForward(double[] inputs)
    {
        foreach (var ws in _weightsList)
        {
            inputs = Activate(inputs, ws);
        }

        return inputs;
    }
}
