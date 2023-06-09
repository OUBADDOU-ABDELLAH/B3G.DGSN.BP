�
KC:\Users\ITAdministrator\source\repos\B3G.DGSN\B3G.DGSN.DOMAIN\JWTHeader.cs
	namespace 	
B3G
 
. 
DGSN 
. 
DOMAIN 
{ 
public		 

class		 
	JWTHeader		 
{

 
public 
string 
_typ 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
_key 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
_alg 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
_kid 
{ 
get  
;  !
set" %
;% &
}' (
} 
} �	
MC:\Users\ITAdministrator\source\repos\B3G.DGSN\B3G.DGSN.DOMAIN\OpenIdToken.cs
	namespace 	
B3G
 
. 
DGSN 
. 
DOMAIN 
{		 
public

 

class

 
OpenIdToken

 
:

 
Object

 %
{ 
[ 	
JsonProperty	 
( 
$str $
)$ %
]% &
public' -
string. 4
_accessToken5 A
;A B
[ 	
JsonProperty	 
( 
$str "
)" #
]# $
public% +
long, 0
	_validity1 :
;: ;
[ 	
JsonProperty	 
( 
$str  
)  !
]! "
public# )
string* 0
_idToken1 9
;9 :
[ 	
JsonProperty	 
( 
$str 
) 
] 
public  &
string' -
_scope. 4
;4 5
[ 	
JsonProperty	 
( 
$str "
)" #
]# $
public% +
string, 2

_tokenType3 =
;= >
} 
} �

OC:\Users\ITAdministrator\source\repos\B3G.DGSN\B3G.DGSN.DOMAIN\RSAJsonWebKey.cs
	namespace 	
B3G
 
. 
DGSN 
. 
DOMAIN 
{		 
public

 

class

 
RSAJsonWebKey

 
{ 
[ 	
JsonProperty	 
( 
$str 
) 
] 
public $
string% +
_use, 0
;0 1
[ 	
JsonProperty	 
( 
$str 
) 
] 
public $
string% +
_kty, 0
;0 1
[ 	
JsonProperty	 
( 
$str 
) 
] 
public $
string% +
_kid, 0
;0 1
[ 	
JsonProperty	 
( 
$str 
) 
] 
public $
string% +
_alg, 0
;0 1
[ 	
JsonProperty	 
( 
$str 
) 
] 
public "
string# )
_n* ,
;, -
[ 	
JsonProperty	 
( 
$str 
) 
] 
public "
string# )
_e* ,
;, -
} 
} �
PC:\Users\ITAdministrator\source\repos\B3G.DGSN\B3G.DGSN.DOMAIN\RSAJsonWebKeys.cs
	namespace 	
B3G
 
. 
DGSN 
. 
DOMAIN 
{ 
public 

class 
RSAJsonWebKeys 
{ 
[

 	
JsonProperty

	 
(

 
$str

 
)

 
]

 
public

 %
RSAJsonWebKey

& 3
[

3 4
]

4 5
keys

6 :
;

: ;
public 
static 
void 
GetIssuerKeys (
(( )
string) /
JwksURL0 7
,7 8
ref9 <
RSAJsonWebKeys= K
keysL P
,P Q
refR U
stringV \
error] b
)b c
{ 	
keys 
= 
null 
; 
HttpClientHandler 
handler %
=& '
new( +
HttpClientHandler, =
(= >
)> ?
;? @

HttpClient 
client 
= 
new  #

HttpClient$ .
(. /
handler/ 6
)6 7
;7 8
string 
url 
= 
JwksURL  
;  !
HttpResponseMessage 
response  (
=) *
client+ 1
.1 2
GetAsync2 :
(: ;
url; >
)> ?
.? @
Result@ F
;F G
if 
( 
response 
. 

StatusCode #
!=$ &
System' -
.- .
Net. 1
.1 2
HttpStatusCode2 @
.@ A
OKA C
)C D
{ 
error 
= 
response  
.  !
ReasonPhrase! -
;- .
return 
; 
} 
using 
( 
HttpContent 
content &
=' (
response) 1
.1 2
Content2 9
)9 :
{ 
string 
jsonResp 
=  !
content" )
.) *
ReadAsStringAsync* ;
(; <
)< =
.= >
Result> D
;D E
if 
( 
string 
. 
IsNullOrEmpty (
(( )
jsonResp) 1
)1 2
)2 3
{ 
error 
= 
$str H
;H I
return 
; 
} 
try 
{   
keys!! 
=!! 
JsonConvert!! &
.!!& '
DeserializeObject!!' 8
<!!8 9
RSAJsonWebKeys!!9 G
>!!G H
(!!H I
jsonResp!!I Q
)!!Q R
;!!R S
}"" 
catch## 
(## 
	Exception##  
e##! "
)##" #
{$$ 
error%% 
=%% 
e%% 
.%% 
Message%% %
;%%% &
return&& 
;&& 
}'' 
}(( 
return)) 
;)) 
}** 	
}++ 
},, �
IC:\Users\ITAdministrator\source\repos\B3G.DGSN\B3G.DGSN.DOMAIN\Session.cs
	namespace 	
B3G
 
. 
DGSN 
. 
DOMAIN 
{		 
public

 

class

 
Session

 
{ 
[ 	
Key	 
] 
public 
string 
state 
{ 
get !
;! "
set# &
;& '
}( )
} 
} 