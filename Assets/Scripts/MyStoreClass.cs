#if UNITY_ANDROID || UNITY_IPHONE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;
using System;

public class MyStoreClass : MonoBehaviour, IStoreListener {

	private bool initializationComplete = false;
	private IStoreController controller;

    private IExtensionProvider storeExtensions;

    /*[System.Serializable]
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
	*/
	public GUIManager guiManager;
    // Use this for initialization

    public void InitStore()
    {
        if (!initializationComplete)
        {
            InitializePurchasing();
        }
    }

	//https://unity3d.com/learn/tutorials/topics/ads-analytics/integrating-unity-iap-your-game
	private void InitializePurchasing()
    {
#if UNITY_ANDROID || UNITY_IPHONE
            StandardPurchasingModule module = StandardPurchasingModule.Instance();
           // module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

			builder.AddProduct(GameConstants.PRODUCT_EXTRA_MOVES, ProductType.NonConsumable);
			builder.AddProduct(GameConstants.PRODUCT_INFINITE_REVIVES, ProductType.NonConsumable);
			builder.AddProduct(GameConstants.PRODUCT_REMOVE_ADS, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
#endif
     }

	public bool IsInitialized() {
		return initializationComplete;
	}
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
            initializationComplete = true;
			this.controller = controller;
            this.storeExtensions = extensions;

            if(guiManager != null)
            {
                Debug.Log("WILL GET PRICES");
                GetPriceForProduct(GameConstants.PRODUCT_EXTRA_MOVES);
                GetPriceForProduct(GameConstants.PRODUCT_INFINITE_REVIVES);
                GetPriceForProduct(GameConstants.PRODUCT_REMOVE_ADS);
            } 
       
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
			Debug.Log("INITIALIZATION FAILED");
            Debug.LogError(string.Format("Purchasing failed to initialize. Reason: {0}", error.ToString()));
			initializationComplete = false;
    }

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {

			Debug.Log(string.Format("IAPButton.ProcessPurchase(PurchaseEventArgs {0} - {1})", e,
                e.purchasedProduct.definition.id));

        //onPurchaseComplete.Invoke(e.purchasedProduct);

            SoundEffectsHelper.Instance.PlaySuccessSound();
            //PURCHASE OK
			if(guiManager!=null) {
				guiManager.PurchaseCompleted(e.purchasedProduct.definition.id);
			}


            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            //PURCHASE FAILED
			if(guiManager!=null) {
				guiManager.PurchaseFailed();
			}
			Debug.Log(string.Format("IAPButton.OnPurchaseFailed(Product {0}, PurchaseFailureReason {1})", product,
                reason));

            //onPurchaseFailed.Invoke(product, reason);
            Debug.LogError("Failed purchase not correctly handled for product \"" + product.definition.id +
                                  "\". Add an active IAPButton to handle this failure, or add an IAPListener to receive any unhandled purchase failures.");
        }

		

		public void PurchaseProduct(string productID, GUIManager manager) {

        if (guiManager == null)
        {
            guiManager = manager;
        }
        
		Debug.Log("STORE PURCHASE PRODUCT " + productID);
		if (controller != null)
		{
			// system's products collection.
                Product product = controller.products.WithID(productID);
                
                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    Debug.Log("Try purchase " + productID);
					controller.InitiatePurchase(productID);
                }


			
		}
			else Debug.Log("NO CONTROLLER");
			
		}

        private void GetPriceForProduct(string productID)
        {
            Debug.Log("STORE GET PRICE FOR PRODUCT " + productID);
            if (controller != null)
            {
                // system's products collection.
                Product product = controller.products.WithID(productID);
                if (product != null && product.availableToPurchase && guiManager!=null)
                {
                    guiManager.UpdatePriceForProduct(productID, product.metadata.localizedPriceString);
                }
            }
            
        }

    public void RestorePurchases(GUIManager gui)
    {

#if UNITY_ANDROID || UNITY_IPHONE
        if(guiManager == null)
        {
            guiManager = gui;
        }

        if(controller!=null && this.storeExtensions!=null && (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) )
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                storeExtensions.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
                    if (result)
                    {
                        // This does not mean anything was restored,
                        // merely that the restoration process succeeded.
                        Debug.Log("Restore purchases OK");
                    }
                    else
                    {
                        // Restoration failed.
                        Debug.Log("Restore purchases NOK");
                    }
                });
            }
            
        }
#endif
    }
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
#endif