using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

public class testMono : MonoBehaviour
{
    async void Start()
    {
        await Task.Run(testAwaitable).ContinueWith(
            t =>
            {
                for (int i = 0; i < 100; i++)
                {
                    test++;
                }  
            }
            );
    }
    
    
    public IEnumerator SomeAsyncTest(){
        async Awaitable TestImplementation(){
            // test something with async / await support here
        };
        return TestImplementation();
    }

    
    async Awaitable testAwaitable()
    {
        await Awaitable.MainThreadAsync();
        for (int i = 0; i < 100; i++)
        {
            test++;
        }
        
    }

    private int test = 0;
    async Task Test()
    {
        for (int i = 0; i < 100; i++)
        {
            test++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
