using System;
using System.Collections.Generic;

public class GenericObjectPool<T> where T : class
{
    private readonly Func<T> _createFunc; // 새 객체 생성 함수
    private readonly Action<T> _onGet;    // 풀에서 가져올 때 처리
    private readonly Action<T> _onRelease;// 풀에 반환할 때 처리
    private readonly Stack<T> _stack;

    public GenericObjectPool(Func<T> createFunc, Action<T> onGet = null, Action<T> onRelease = null, int initialSize = 0)
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        _createFunc = createFunc;
        _onGet = onGet;
        _onRelease = onRelease;
        _stack = new Stack<T>(initialSize);

        // 초기화
        for (int i = 0; i < initialSize; i++)
        {
            _stack.Push(_createFunc());
        }
    }

    /// <summary>
    /// 객체 가져오기
    /// </summary>
    public T Get()
    {
        T obj = _stack.Count > 0 ? _stack.Pop() : _createFunc();
        _onGet?.Invoke(obj);
        return obj;
    }

    /// <summary>
    /// 객체 반환
    /// </summary>
    public void Release(T obj)
    {
        _onRelease?.Invoke(obj);
        _stack.Push(obj);
    }

    /// <summary>
    /// 풀 안 객체 개수
    /// </summary>
    public int Count => _stack.Count;
}