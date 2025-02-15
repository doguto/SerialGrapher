# SerialGrapher
マイコンからシリアル通信を介して二変数の数値を受け取り、それらを元にグラフを描写するアプリです。

## How to use（PC）
起動したら適切なCOMポート番号と最大プロット数をtextBoxに入力し、それぞれ確定ボタンを押します。
ここで不適切な値（0以下の値）を入れるとグラフの描写に移ることができません。

設定を確定させたら開始ボタンからグラフの描写を開始してください。

## How to use（MicroComputer）
以下のようなコードによってPC側に数値を送信します。
```C++
UnbufferedSerial pc(USBTX, USBRX, 115200);

char outPut[50];
int length;
float x;
float y;

int main() {
    length = snprintf(outPut, sizeof(outPut), "init\n");
    pc.write(outPut, length);
    thread_sleep_for(500);

    while (true) {
        x += 0.01;
        y = 1/x;
        length = snprintf(outPut, sizeof(outPut), "%f,%f\n", x, y);
        pc.write(outPut, length);
        thread_sleep_for(10ms);
    }
}
```
注意点として、最初に必ず```"init\n"```を送信するようにしてください。
initをトリガーとしてグラフが起動されます。

## Used Tech
* C#
* Windows Form
* LiveCharts