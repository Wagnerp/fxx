#fxx 

![fxx-cli in action](http://ombsoft.com/hash.png)

*Because f(x)=x*

Identify file versions and product versions based upon file hashes.

- Hashes files individually using SHA-1 
- Constructs total (snowball) hashes of all files in a given installation
- Can store hashes into any CRUD capable database - currently designed against Apache's CouchDB
- Installations can be identified by comparing against hashes stored in the databases
- Designed to be resilient to ad-hoc patching or modifications of single files in a given product (fxx can determine whether an installation is a modified version of known product)
- Details of specific patches and one-off modifications can be hashed and identified later as part of an installation (e.g. "This is patch 7 of product XYZ")

## Rationale
Sometimes it can be difficult to tell whether a customer (or otherwise) is still using a stock installation of a product - or whether they've received ad-hoc patches or configuration changes. These situations become even more problematic if product and file versioning is inconsistent - or maybe if you have a rogue DLL or two that have identical file versions but are significant patches. 

Enter fxx: this tool allows the construction of a hash repository to identify different installations and file versions, without depending on the product *telling* you what version is. You can be alerted immediately to any unknown or modified files. This removes any element of uncertainty about the exact state of a given installation. 

##Status

fxx is very much work in progress. The command line version (fxx-cli) is now usable for identifying and submitting product details. However, it is still missing some important features:

- There is not yet any unattended mode (e.g. for submitting hashes from build machines)
- There is no file extension filtering
- The client fails to warn if a file matches on hash but not on name (i.e. a file of the same hash is known but does not match the filename on disk)
