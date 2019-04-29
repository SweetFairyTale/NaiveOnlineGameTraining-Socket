/*
 * Common自定义类库(基于.NET 2.0 — Unity2017支持的版本)
 * 可在多个应用中共享公共定义
 * 
 * 为使服务器一个Controller层能处理多个Client的请求
 * 定义枚举类型RequsetCode(指定由哪个controller处理)和ActionCode(指定由controller中的哪个方法来处理)
 * 用于匹配操作码指定对应操作
 */