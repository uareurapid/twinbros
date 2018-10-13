using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;

public class MyStoreClass : MonoBehaviour, IStoreListener {

	private bool initializationComplete = false;
	private bool unityPurchasingInitialized = false;
	private IStoreController controller;

	[System.Serializable]
    public class OnPurchaseCompletedEvent : UnityEvent<Product>
    {
    };

    [System.Serializable]
    public class OnPurchaseFailedEvent : UnityEvent<Product, PurchaseFailureReason>
    {
    };

	//[Tooltip("Event fired after a successful purchase of this product")]
    private OnPurchaseCompletedEvent onPurchaseComplete;

    //[Tooltip("Event fired after a failed purchase of this product")]
    private OnPurchaseFailedEvent onPurchaseFailed;
	// Use this for initialization
	void Start () {

		InitializePurchasing();
	}

	private void InitializePurchasing()
    {
            StandardPurchasingModule module = StandardPurchasingModule.Instance();
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

			builder.AddProduct(GameConstants.PRODUCT_EXTRA_MOVES, ProductType.NonConsumable);
			builder.AddProduct(GameConstants.PRODUCT_INFINITE_REVIVES, ProductType.NonConsumable);
			builder.AddProduct(GameConstants.PRODUCT_REMOVE_ADS, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);

            unityPurchasingInitialized = true;
     }

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
            initializationComplete = true;
			this.controller = controller;
       
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
            Debug.LogError(string.Format("Purchasing failed to initialize. Reason: {0}", error.ToString()));
			initializationComplete = false;
    }

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {

			Debug.Log(string.Format("IAPButton.ProcessPurchase(PurchaseEventArgs {0} - {1})", e,
                e.purchasedProduct.definition.id));

			onPurchaseComplete.Invoke(e.purchasedProduct);


            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            
			Debug.Log(string.Format("IAPButton.OnPurchaseFailed(Product {0}, PurchaseFailureReason {1})", product,
                reason));

            onPurchaseFailed.Invoke(product, reason);
            Debug.LogError("Failed purchase not correctly handled for product \"" + product.definition.id +
                                  "\". Add an active IAPButton to handle this failure, or add an IAPListener to receive any unhandled purchase failures.");
        }

		

		public void PurchaseProduct(string productID) {

		Debug.Log("STORE PURCHASE PRODUCT " + productID);
		if (controller != null)
		{
			Debug.Log("Try purchase " + productID);
			controller.InitiatePurchase(productID);
		}
			else Debug.Log("NO CONTROLLER");
			
		}
}
