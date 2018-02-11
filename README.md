# Spider(网络爬虫)

- [github项目](https://github.com/wang128/Spider)，利用 C# 编程实现。
- 我一直都想做一个爬虫程序，通过这次对爬虫程序的实现，我学到了很多，对多线程编程有了一些初步认识，了解了爬虫程序的原理。
- 一开始我实现的只是将爬取的网页下载到对应目录（先根据网址建好文件夹），爬取图片的是后来加上去的。

## 工作流程
![](https://github.com/wang128/Spider/blob/master/src/process.jpg)

## 运行环境
 - .NET Framework
 
## 下载
- 点击上方 program 文件夹，再点击 Spider.rar 进去新的页面，再次点击Download即可下载
- 在你自己电脑最喜欢的地方解压这个压缩包，点击里面的 Spider/Debug/bin/Debug 下的.exe文件运行程序

## 改进方向
- 爬虫与数据库
- 数据库保存爬出来的数据
- 数据库简单整理爬出来的数据
- 应对“反爬”的网站
- 减轻被爬网站的负荷
- 网页搜索策略（我用的是广度优先搜索，还有深度优先搜索和最佳优先搜索）
- 解析html可以用正则匹配（我是自己写了一个独立的解析html的类）

## 运行结果
- 下载网页<br/>
![](https://github.com/wang128/Spider/blob/master/src/test1.jpg)<br/>
![](https://github.com/wang128/Spider/blob/master/src/result1.jpg)
- 下载图片<br/>
![](https://github.com/wang128/Spider/blob/master/src/test2.jpg)<br/>
![](https://github.com/wang128/Spider/blob/master/src/result2.jpg)
