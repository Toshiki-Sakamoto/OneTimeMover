namespace Core.Common
{
    /// <summary>
    /// リフレクションでInjectを行う
    ///     ※ VContainer参照
    ///
    /// キャッシュはしていないので、毎回リフレクションを行う
    /// </summary>
    public class ReflectionInjector : IInjector
    {
        public void Inject(object instance, IServiceResolver resolver)
        {
            InjectMethods(instance, resolver);
        }

        /// <summary>
        /// Injectがついているメソッドの呼び出し
        /// </summary>
        private void InjectMethods(object instance, IServiceResolver resolver)
        {
            var type = instance.GetType();

            // 子 -> 親の順でメソッドを呼び出す
            while (type != null && type != typeof(object))
            {
                // インジェクト対象のメソッドを取得
                var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                foreach (var method in methods)
                {
                    // メソッドにInject属性が付いているか確認
                    if (method.GetCustomAttributes(typeof(InjectAttribute), false).Length <= 0) continue;
                    
                    // メソッドに必要なパラメータを取得
                    var parameters = method.GetParameters();
                    var args = new object[parameters.Length];

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        // パラメータの型からインスタンスを解決
                        args[i] = resolver.Resolve(parameters[i].ParameterType);
                    }

                    // メソッドを呼び出す
                    method.Invoke(instance, args);
                }

                type = type.BaseType;
            }
        }
    }
}