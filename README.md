fxx 
===
*Because f(x)=x*

Identify file versions and product versions based upon file hashes.

- Hashes files individually using SHA-1 
- Constructs total (snowball) hashes of all files in a given installation
- Can store hashes into any CRUD capable database - currently designed against Apache's CouchDB
- Installations can be identified by comparing against hashes stored in the databases
- Designed to be resilient to ad-hoc patching or modifications of single files in a given product (fxx can determine whether an installation is a modified version of known product)
- Details of specific patches and one-off modifications can be hashed and identified later as part of an installation (e.g. "This is patch 7 of product XYZ")

**Status**
fxx is very much work in progress. The skeleton is functional but there is currently no usable interface. Error handling is also currently MIA.
