# LocalDataManager

이 플러그인은 게임 내 로컬 데이터를 쉽게 관리하는 플러그인 입니다.


## Description

게임 내에서 사용되는 로컬 데이터를 Json으로 변환 후 PlayerPrefs에 저장합니다.

## Installation

#### Requiement

- 2018.1 and later (recommended)

#### Package Manager

manifest.json 파일을 당신의 프로젝트 폴더 내 Package folder 에서 찾고, 아래와 같이 수정하세요.

```
{
  "dependencies": {
      "com.youngpackage.data": "https://github.com/Youngchangoon/LocalDataManager.git",
  },
  "scopedRegistries": [
    {
      "name": "Packages from jillejr",
      "url": "https://npm.cloudsmith.io/jillejr/newtonsoft-json-for-unity/",
      "scopes": [
        "jillejr"
      ]
    }
  ]
}
```

## Usage

1. `IDataBase`를 상속하는 data class를 만듭니다.
2. Class 안에 관리하고 싶은 데이터 타입을 만듭니다.
3. 생성자를 만들고, 기본 값들을 넣어줍니다.

```
public class UserData : IDataBase
{
    public int test1;
    public string test2;

    public UserData()
    {
        test1 = 1000;
        test2 = "YEAH";
    }
}
```

게임 초기화 부분에서 PreInit() 함수를 호출합니다.
상황에 맞게 함수를 호출하여 사용합니다.

```
void Awake()
{
    // [Init]
    GameDataManager.PreInit(new UserData()); 

    // [Load]
    var userInfo = GameDataManager.GetData<UserData>();

    // [Edit]
    userInfo.test1 = 2000;

    // [Save]
    GameDataManager.SaveData<UserData>();
}
```

### API


- PreInit(): 게임 내에서 초기화 부분에 한번 호출 합니다.
- GetData<T>(): 초기화 후, 캐싱된 데이터를 불러옵니다.
- SetData<T>(T data): 새로운 Data로 대체합니다.
- SaveData<T>(): Json으로 변환후, PlayerPrefs에 String으로 저장합니다.


