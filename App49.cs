// *** 時變頻率數值計算(Time-Variant-Frequency Numerical Computations) ***  

// 三階微分方程式: N(t)*y'''(t)+M(t)*y''(t)+C(t)*y'(t)+K(t)*y(t)= f(t) 
// 由齊次解，狀態空間(State-Spce)時變(Time-Variant)矩陣微分方程式，求得系統矩陣A，
// 進而求得A = Q * D * Qi。其中 D，Q，Qi 分別為特徵矩陣、模態矩陣、逆模態矩陣。
// 進而求得訊號響應值 [y''|y'|y]g = Hexp(D, Q, t) * d + [y''|y'|y]p , 
// 稱 Hexp(D, Q, t)為狀態空間響應函數，d是由初始值或是
// 邊界值而定的係數向量，兩者的預設值均為複數矩陣和複數的向量。    
// 本求解法可對應於一般的Laplace、Fourier、Z Transform或是捲積積分法等等，
// 上述都是間接的求解方法。但本法則是直接求取系統響應值(矩陣微分方程式求解法)。

using Matrix_0;

int m = 4;  // 空間維度有m個自由度。
int r = 3;  // 狀態維度有r個自由度，即r階(Order)微分方程，A 是 12 X 12 矩陣。 

// 建構初始(空)矩陣 N, M、C、K、Zero、Id。
ReMatrix N = (new Zero(m)).GetMatrix;
ReMatrix M = (new Zero(m)).GetMatrix;
ReMatrix C = (new Zero(m)).GetMatrix;
ReMatrix K = (new Zero(m)).GetMatrix;
ReMatrix Zero = (new Zero(m)).GetMatrix;
ReMatrix Id = (new Iden(m)).GetMatrix;

// 實數系統矩陣A，特徵矩陣D、模態矩陣Q。
ReMatrix A;     
CxMatrix D;    
CxMatrix Q; 

// 狀態響應。速度，變位，加速度。(Step = 0.001秒，共計 t = 0.05秒) 
double step = 0.001;
int iRow = (int)(0.05 / step + 1);

// 建構時間軸上的儲存矩陣，增加時間t壹行，故儲存矩陣有(m * r) + 1行。
int iColD = m * r + 1;
CxMatrix CxVal = new CxMatrix(iRow, iColD);
ReMatrix ReVal = new ReMatrix(iRow, iColD);

for (int i = 0; i != iRow; i++)
{
    double t = step * i;

    // 建構 N、M、C、K 變數矩陣。
    N.Matrix[0, 0] = -2.7 * t * t * Math.Sin(1.3 * t);
    N.Matrix[0, 1] = -5.5;
    N.Matrix[0, 2] = 0;
    N.Matrix[0, 3] = 5.5;
    N.Matrix[1, 0] = 3.5;
    N.Matrix[1, 1] = -8.5;
    N.Matrix[1, 2] = -9.8 * t * t;
    N.Matrix[1, 3] = -4.8;
    N.Matrix[2, 0] = 6.7;
    N.Matrix[2, 1] = 27.9;
    N.Matrix[2, 2] = 8.5;
    N.Matrix[2, 3] = -20.5 * t * t * Math.Cos(1.9 * t);
    N.Matrix[3, 0] = -1.5 * t * Math.Cos(1.9 * t);
    N.Matrix[3, 1] = 4.8;
    N.Matrix[3, 2] = 0;
    N.Matrix[3, 3] = 1.5 * t * t * t;
    //  End of N Matrix  

    M.Matrix[0, 0] = 19;
    M.Matrix[0, 1] = -1.5;
    M.Matrix[0, 2] = -2 + 13.3 * Math.Sin(0.85 * t);
    M.Matrix[0, 3] = 1.1;
    M.Matrix[1, 0] = -1;
    M.Matrix[1, 1] = 15;
    M.Matrix[1, 2] = 0;
    M.Matrix[1, 3] = 1.3;
    M.Matrix[2, 0] = -10 - 2.7 * Math.Cos(1.3 * t);
    M.Matrix[2, 1] = -3;
    M.Matrix[2, 2] = 27;
    M.Matrix[2, 3] = 4.5;
    M.Matrix[3, 0] = 5.5;
    M.Matrix[3, 1] = 2.7;
    M.Matrix[3, 2] = -2.3 * t;
    M.Matrix[3, 3] = -3.5 * t * t;
    // End of M Matrix 

    C.Matrix[0, 0] = 35;
    C.Matrix[0, 1] = -1 - 13.2 * Math.Sin(0.35 * t);
    C.Matrix[0, 2] = -0.5;
    C.Matrix[0, 3] = 2.5;
    C.Matrix[1, 0] = -1.5;
    C.Matrix[1, 1] = 40;
    C.Matrix[1, 2] = -1.5;
    C.Matrix[1, 3] = 0;
    C.Matrix[2, 0] = -1.2 + 22.5 * Math.Cos(1.95 * t);
    C.Matrix[2, 1] = -1.5;
    C.Matrix[2, 2] = 75;
    C.Matrix[2, 3] = 0;
    C.Matrix[3, 0] = -27.5;
    C.Matrix[3, 1] = 18.3;
    C.Matrix[3, 2] = 9.5;
    C.Matrix[3, 3] = -50.9 * t * Math.Sin(2.5 * t);
    // End of C Matrix 

    K.Matrix[0, 0] = 60;
    K.Matrix[0, 1] = -8;
    K.Matrix[0, 2] = -2 - 332 * Math.Sin(1.37 * t);
    K.Matrix[0, 3] = -2.7;
    K.Matrix[1, 0] = -16;
    K.Matrix[1, 1] = 180;
    K.Matrix[1, 2] = -120;
    K.Matrix[1, 3] = 100;
    K.Matrix[2, 0] = -20;
    K.Matrix[2, 1] = -100 + 579 * Math.Cos(0.24 * t);
    K.Matrix[2, 2] = 300;
    K.Matrix[2, 3] = 20;
    K.Matrix[3, 0] = 1.5 * Math.Sin(t);
    K.Matrix[3, 1] = -9.8;
    K.Matrix[3, 2] = 150;
    K.Matrix[3, 3] = 11.5 * t * t * Math.Cos(t);
    // End of K Matrix 

    // 隨時間變化的系統矩陣A，(12X12矩陣)(m = 4, r = 3)。   
    ReMatrix Ni = ~N;
        
    A = ((-1.0 * Ni * M) & (-1.0 * Ni * C) & (-1.0 * Ni * K)) | 
        (Id & Zero & Zero) | (Zero & Id & Zero);
    
    Console.WriteLine(" i = {0}   t = {1}  ", i, t);
    Console.WriteLine(
        "\n*** 因為計算特徵矩陣和模態矩陣的時間較長，顯示執行狀況:\n");

    // 隨時間變化的系統特徵矩陣 D，模態矩陣 Q 。 
    D = (new EIG(A)).CxMatrixD;
    Q = (new EIG(A)).CxMatrixQ;
    Console.WriteLine("\n   *** 計算特徵值和特徵向量之後 : ***\n");

    // 將時間轉爲複數值。
    CxScalar cxScalar = new CxScalar(t, 0);
    // 隨時間變化的特徵矩陣。
    CxVal[i, 0] = new CxMatrix(cxScalar);
    CxVal[i, 1] = D[0, 0];
    CxVal[i, 2] = D[1, 1];
    CxVal[i, 3] = D[2, 2];
    CxVal[i, 4] = D[3, 3];
    CxVal[i, 5] = D[4, 4];
    CxVal[i, 6] = D[5, 5];
    CxVal[i, 7] = D[6, 6];
    CxVal[i, 8] = D[7, 7];
    CxVal[i, 9] = D[8, 8];
    CxVal[i, 10] = D[9, 9];
    CxVal[i, 11] = D[10, 10];
    CxVal[i, 12] = D[11, 11];

    // 隨時間變化的角頻率(實數值轉爲矩陣)。       
    double[,] tMatrix = { { t } };

    ReVal[i, 0] = (ReMatrix)tMatrix;
    ReVal[i, 1] = D[0, 0].Im;
    ReVal[i, 2] = D[1, 1].Im;
    ReVal[i, 3] = D[2, 2].Im;
    ReVal[i, 4] = D[3, 3].Im;
    ReVal[i, 5] = D[4, 4].Im;
    ReVal[i, 6] = D[5, 5].Im;
    ReVal[i, 7] = D[6, 6].Im;
    ReVal[i, 8] = D[7, 7].Im;
    ReVal[i, 9] = D[8, 8].Im;
    ReVal[i, 10] = D[9, 9].Im;
    ReVal[i, 11] = D[10, 10].Im;
    ReVal[i, 12] = D[11, 11].Im;

}

Console.WriteLine("\n***  時間和特徵值(有十二組)，合計十三組複數值  ***");
Console.WriteLine("\n{0}\n\n", new PR(CxVal));

Console.WriteLine("\n***  特徵值矩陣的虛數值即角頻率  ***");
Console.WriteLine("       時間 t      ....   十二個角頻率 ");
Console.WriteLine("\n{0}\n", new PR(ReVal));

// 轉爲序列方式，以便使用python程式繪圖。 
Console.WriteLine("\n時間序列:   t\n{0}\n", new PR4(ReVal, 0));
Console.WriteLine("\n角頻率序列:w0\n{0}\n", new PR4(ReVal, 1));
Console.WriteLine("\n角頻率序列:w1\n{0}\n", new PR4(ReVal, 2));
Console.WriteLine("\n角頻率序列:w2\n{0}\n", new PR4(ReVal, 3));
Console.WriteLine("\n角頻率序列:w3\n{0}\n", new PR4(ReVal, 4));
Console.WriteLine("\n角頻率序列:w4\n{0}\n", new PR4(ReVal, 5));
Console.WriteLine("\n角頻率序列:w5\n{0}\n", new PR4(ReVal, 6));
Console.WriteLine("\n角頻率序列:w6\n{0}\n", new PR4(ReVal, 7));
Console.WriteLine("\n角頻率序列:w7\n{0}\n", new PR4(ReVal, 8));
Console.WriteLine("\n角頻率序列:w8\n{0}\n", new PR4(ReVal, 9));
Console.WriteLine("\n角頻率序列:w9\n{0}\n", new PR4(ReVal, 10));
Console.WriteLine("\n角頻率序列:w10\n{0}\n", new PR4(ReVal, 11));
Console.WriteLine("\n角頻率序列:w11\n{0}\n", new PR4(ReVal, 12));

/*輸出結果:

***  時間和特徵值(有十二組)，合計十三組複數值  ***

 0.00000 +  0.00000i, -11.77658 +   0.00000i,  -3.74980 +   0.00000i,
-2.20404 +  2.54725i,  -2.20404 -   2.54725i,   2.97024 +   0.49164i,
 2.97024 -  0.49164i,   1.50230 +   2.28138i,   1.50230 -   2.28138i,
-0.03875 +  2.53751i,  -0.03875 -   2.53751i,  -1.03565 +   1.15840i, 
    .
    .
    .
*/