#fxx 

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

fxx is very much work in progress. The skeleton is functional but there is currently no usable interface.
