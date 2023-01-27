using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DataPlotter : MonoBehaviour
{

    // arquivo de entrada dos dados.
    public string inputFileName;

    // lista para armazenar os dados do leitor do CSV
    private List<Dictionary<string, object>> pointList;

    // Indices das colunas para ser atribuídos
    public int XcolumnIndex = 1;
    public int YcolumnIndex = 3;
    public int ZcolumnIndex = 4;
    public int ClasscolumnIndex = 0;

    public TMP_Text eixoXText;
    public TMP_Text eixoYText;
    public TMP_Text eixoZText;

    // Nomes das colunas
    public string xColumnName;
    public string yColumnName;
    public string zColumnName;
    public string className;

    public float plotScale = 10;

    // O prefab para os pontos de dados que serão instanciados
    public GameObject PointPrefab;

    // Objeto que conterá prefabs instanciados na hierarquia 
    public GameObject PointHolder;

    // Inicialização
    void Start()
    {

        // Setar lista de pontos para resultados de função leitora de CSV com inputFileName arqumento
        pointList = CSVReader.Read(inputFileName);

        Debug.Log(pointList);

        // Declara lista de strings, Declare list of strings,preenche com chaves (nome de colunas)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Printa número de chaves  (usando .count)
        Debug.Log("There are " + columnList.Count + " columns in the CSV");

        foreach (string key in columnList)
            Debug.Log("Column name is " + key);

        // Atribui nome de coluna da columnList ao nome de variáveis 
        xColumnName = columnList[XcolumnIndex];
        yColumnName = columnList[YcolumnIndex];
        zColumnName = columnList[ZcolumnIndex];
        className = columnList[ClasscolumnIndex];

        //Nome dos eixos x, y, z
        eixoXText.text = xColumnName;
        eixoYText.text = yColumnName;
        eixoZText.text = zColumnName;

        // Pega máximo de cada eixo
        float xMax = FindMaxValue(xColumnName);
        float yMax = FindMaxValue(yColumnName);
        float zMax = FindMaxValue(zColumnName);

        // Pega mínimod e cada eixo
        float xMin = FindMinValue(xColumnName);
        float yMin = FindMinValue(yColumnName);
        float zMin = FindMinValue(zColumnName);


        //Intera pela lista de pontos
        for (var i = 0; i < pointList.Count; i++)
        {
            // Pega valor em pointListna linha, na nome da coluna, normaliza.
            float x =
                (System.Convert.ToSingle(pointList[i][xColumnName]) - xMin)
                / (xMax - xMin);

            float y =
                (System.Convert.ToSingle(pointList[i][yColumnName]) - yMin)
                / (yMax - yMin);

            float z =
                (System.Convert.ToSingle(pointList[i][zColumnName]) - zMin)
                / (zMax - zMin);


            // Instancia como variável gameobject, assim é possível manipular com loop.
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(x, y, z) * plotScale,
                    Quaternion.identity);
          
            // Cria labels dos pontos cóm número da classe
            GameObject textobj = dataPoint.transform.GetChild(0).GetChild(0).gameObject;
            TMP_Text labelClass = textobj.GetComponent<TMP_Text>();
            float number = System.Convert.ToSingle(pointList[i][className]);
            labelClass.text = number.ToString();

            // Cria child do objeto PointHolder, para manter pontos com container na hierarquia.
            dataPoint.transform.parent = PointHolder.transform;

            // Atribui valores originais ao dataPointName 
            string dataPointName =
                pointList[i][xColumnName] + " "
                + pointList[i][yColumnName] + " "
                + pointList[i][zColumnName];


            // Atribui nome ao prefab
            dataPoint.transform.name = dataPointName;


            if (pointList[i][className].Equals(1))
            {
                // pega a cor do material e setar para uma nova cor
                dataPoint.GetComponent<Renderer>().material.color =
                     new Color(0, 0, 1, 1.0f);
            }

            if (pointList[i][className].Equals(2))
            {
                // pega a cor do material e setar para uma nova cor
                dataPoint.GetComponent<Renderer>().material.color =
                     new Color(0, 1, 0, 1.0f);
            }
            if (pointList[i][className].Equals(3))
            {
                // pega a cor do material e setar para uma nova cor
                dataPoint.GetComponent<Renderer>().material.color =
                     new Color(1, 0, 0, 1.0f);
            }

        }



    }

    private float FindMaxValue(string columnName)
    {
        //seta valor incial ao primeiro valor.
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Intera pelo dicionário, sobrescreve o máximo valor existente, se o novo valor é maior.
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //retorna o valor máximo
        return maxValue;
    }

    private float FindMinValue(string columnName)
    {

        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Intera pelo dicionário, sobrescreve o mínimo valor existente, se o novo valor é maior.
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }

}