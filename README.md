# XiaoMing
### 开发环境
- VS2015或者VS2017(win10)
- .NET Framework 4.0
- CefSharp.WinForms 49.0.0
- 目标平台x86或者x64
### 开发注意事项
- clone下代码后，第一次打开项目需要重新生成一次
- 调式或运行时Any CPU需要改为x86或者x64
- 每一项功能都要分别考虑2种不同内核下的运行
### 目前已完成的功能
- Tab页签的添加和删除
- 切换内核模式（ie和chrome）
- 前进、后退和刷新功能
- Tab页签动态显示网页标题、地址栏显示当前网页URL地址
### 下一阶段任务
- js脚本的注入、监听页面事件及请求
- 在不同电脑环境下测试安装包的安装情况：是否顺利、用户能否接受
- KPI登录能否正常使用以及性能等
### 其他
#### 修改WebBrowser控件的内核版本（默认使用了IE7兼容性模式来浏览网页）
- 你可以通过设置注册表FEATURE_BROWSER_EMULATION 来实现
- 在html头 加标签 强制使用最新的ie渲染```<meta http-equiv="X-UA-Compatible" content="IE=edge">```
- 强制使用最新的ie8渲染```<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8"/>```
#### 效果预览
[简书](http://www.jianshu.com/p/72a32ac3c65d) 

