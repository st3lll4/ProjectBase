namespace App.DAL.EF.DataSeeding;

public static class InitialData
{
    public static readonly (string roleName, Guid? id)[]
        Roles =
        [
            ("Admin", null),
            ("User", null)
        ];

    public static readonly (string name, string firstName, string lastName, string password, Guid? id, string[] roles)[]
        Users =
        [
            ("stella_tukia@hotmail.com", "admin", "stella", "Foo.Bar.1", null, ["Admin"]),
            ("user@normal.ee", "user", "normal", "Foo.Bar.2", null, []),
        ];

    /*public static readonly (string name, Guid? id)[]
        CardTypes =
        [
            ("Album PC", null),
            ("Polaroid", null),
            ("Pre-Order Benefit", null),
            ("Lucky Draw", null),
            ("Special Event", null),
            ("Mini PC", null),
            ("Fansign", null)
        ];

    public static readonly (string StageName, DateTime BirthDate, string FirstName, string Lastname, bool isSolo, Guid?
        id, string[] groups)[]
        Artists =
        [
            // ATEEZ
            ("Hongjoong", new DateTime(1998, 11, 7), "Kim", "Hongjoong", false, null, ["ATEEZ"]),
            ("Seonghwa", new DateTime(1998, 4, 3), "Park", "Seonghwa", false, null, ["ATEEZ"]),
            ("Yunho", new DateTime(1999, 3, 23), "Jeong", "Yunho", false, null, ["ATEEZ"]),
            ("Yeosang", new DateTime(1999, 6, 15), "Kang", "Yeosang", false, null, ["ATEEZ"]),
            ("San", new DateTime(1999, 7, 10), "Choi", "San", false, null, ["ATEEZ"]),
            ("Mingi", new DateTime(1999, 8, 9), "Song", "Mingi", false, null, ["ATEEZ"]),
            ("Wooyoung", new DateTime(1999, 11, 26), "Jung", "Wooyoung", false, null, ["ATEEZ"]),
            ("Jongho", new DateTime(2000, 10, 12), "Choi", "Jongho", false, null, ["ATEEZ"]),

            // STRAY KIDS
            ("Bang Chan", new DateTime(1997, 10, 3), "Christopher", "Bang", false, null, ["Stray Kids"]),
            ("Lee Know", new DateTime(1998, 10, 25), "Lee", "Minho", false, null, ["Stray Kids"]),
            ("Changbin", new DateTime(1999, 8, 11), "Seo", "Changbin", false, null, ["Stray Kids"]),
            ("Hyunjin", new DateTime(2000, 3, 20), "Hwang", "Hyunjin", false, null, ["Stray Kids"]),
            ("Han", new DateTime(2000, 9, 14), "Han", "Jisung", false, null, ["Stray Kids"]),
            ("Felix", new DateTime(2000, 9, 15), "Lee", "Felix", false, null, ["Stray Kids"]),
            ("Seungmin", new DateTime(2000, 9, 22), "Kim", "Seungmin", false, null, ["Stray Kids"]),
            ("I.N", new DateTime(2001, 2, 8), "Yang", "Jeongin", false, null, ["Stray Kids"]),

            // SOLO 
            ("BIBI", new DateTime(1998, 9, 27), "Kim", "Hyungseo", true, null, []),
            ("B.I", new DateTime(1996, 10, 22), "Kim", "Hanbin", true, null, []),

            // GIDLE
            ("Soyeon", new DateTime(1998, 8, 26), "Jeon", "Soyeon", false, null, ["G(I)-DLE"]),
            ("Miyeon", new DateTime(1997, 1, 31), "Cho", "Miyeon", false, null, ["G(I)-DLE"]),
            ("Minnie", new DateTime(1997, 10, 23), "Nicha", "Yontararak", false, null, ["G(I)-DLE"]),
            ("Yuqi", new DateTime(1999, 9, 23), "Song", "Yuqi", false, null, ["G(I)-DLE"]),
            ("Shuhua", new DateTime(2000, 1, 6), "Yeh", "Shuhua", false, null, ["G(I)-DLE"]),

            // SHINee
            ("Onew", new DateTime(1989, 12, 14), "Lee", "Jinki", false, null, ["SHINee"]),
            ("Jonghyun", new DateTime(1990, 4, 8), "Kim", "Jonghyun", false, null, ["SHINee"]),
            ("Key", new DateTime(1991, 9, 23), "Kim", "Kibum", false, null, ["SHINee"]),
            ("Minho", new DateTime(1991, 12, 9), "Choi", "Minho", false, null, ["SHINee"]),
            ("Taemin", new DateTime(1993, 7, 18), "Lee", "Taemin", false, null, ["SHINee"]),

            // TWICE
            ("Nayeon", new DateTime(1995, 9, 22), "Im", "Nayeon", false, null, ["TWICE"]),
            ("Jeongyeon", new DateTime(1996, 11, 1), "Yoo", "Jeongyeon", false, null, ["TWICE"]),
            ("Momo", new DateTime(1996, 11, 9), "Hirai", "Momo", false, null, ["TWICE"]),
            ("Sana", new DateTime(1996, 12, 29), "Minatozaki", "Sana", false, null, ["TWICE"]),
            ("Jihyo", new DateTime(1997, 2, 1), "Park", "Jihyo", false, null, ["TWICE"]),
            ("Mina", new DateTime(1997, 3, 24), "Myoui", "Mina", false, null, ["TWICE"]),
            ("Dahyun", new DateTime(1998, 5, 28), "Kim", "Dahyun", false, null, ["TWICE"]),
            ("Chaeyoung", new DateTime(1999, 4, 23), "Son", "Chaeyoung", false, null, ["TWICE"]),
            ("Tzuyu", new DateTime(1999, 6, 14), "Chou", "Tzuyu", false, null, ["TWICE"]),
        ];

    public static readonly (string GroupName, string FandomName, Guid? id)[]
        Groups =
        [
            ("ATEEZ", "ATINY", null),
            ("Stray Kids", "STAY", null),
            ("G(I)-DLE", "NEVERLAND", null),
            ("TWICE", "ONCE", null),
            ("SHINee", "Shawol", null)
        ];

    public static readonly (string Title, DateTime ReleaseDate, string? Artist, string? Group, Guid? id)[]
        Albums =
        [
            // ATEEZ 
            ("TREASURE EP.1: All to Zero", new DateTime(2018, 10, 24), null, "ATEEZ", null),
            ("ZERO: FEVER Part.1", new DateTime(2020, 7, 29), null, "ATEEZ", null),
            ("THE WORLD EP.1: MOVEMENT", new DateTime(2022, 7, 29), null, "ATEEZ", null),

            // Stray Kids 
            ("I am NOT", new DateTime(2018, 3, 26), null, "Stray Kids", null),
            ("GO生 (GO LIVE)", new DateTime(2020, 6, 17), null, "Stray Kids", null),
            ("★★★★★ (5-STAR)", new DateTime(2023, 6, 2), null, "Stray Kids", null),

            // BIBI 
            ("Lowlife Princess: Noir", new DateTime(2022, 11, 18), "BIBI", null, null),

            // B.I 
            ("WATERFALL", new DateTime(2021, 6, 1), "B.I", null, null),
            ("COSMOS", new DateTime(2021, 11, 11), "B.I", null, null),
            ("TO DIE FOR", new DateTime(2023, 6, 1), "B.I", null, null),

            // (G)I-DLE
            ("I NEVER DIE", new DateTime(2022, 3, 14), null, "G(I)-DLE", null),
            ("I FEEL", new DateTime(2023, 5, 15), null, "G(I)-DLE", null),

            // TWICE
            ("Formula of Love: O+T=<3", new DateTime(2021, 11, 12), null, "TWICE", null),
            ("READY TO BE", new DateTime(2023, 3, 10), null, "TWICE", null),

            // SHINee
            ("The Story of Light Ep.1", new DateTime(2018, 5, 28), null, "SHINee", null),
            ("Don't Call Me", new DateTime(2021, 2, 22), null, "SHINee", null),
            ("HARD", new DateTime(2023, 6, 26), null, "SHINee", null),
        ];

    public static readonly (string Name, string albumName, Guid? id)[]
        AlbumVersions =
        [
            // ATEEZ
            ("A Ver.", "TREASURE EP.1: All to Zero", null),
            ("Z Ver.", "TREASURE EP.1: All to Zero", null),

            ("Diary Ver.", "ZERO: FEVER Part.1", null),
            ("Inception Ver.", "ZERO: FEVER Part.1", null),
            ("Thanxx Ver.", "ZERO: FEVER Part.1", null),

            ("A Ver.", "THE WORLD EP.1: MOVEMENT", null),
            ("Diary Ver.", "THE WORLD EP.1: MOVEMENT", null),
            ("Z Ver.", "THE WORLD EP.1: MOVEMENT", null),

            // Stray Kids
            ("I am Ver.", "I am NOT", null),
            ("NOT Ver.", "I am NOT", null),

            ("Standard Ver.", "GO生 (GO LIVE)", null),
            ("Limited Ver.", "GO生 (GO LIVE)", null),

            ("A Ver.", "★★★★★ (5-STAR)", null),
            ("B Ver.", "★★★★★ (5-STAR)", null),
            ("C Ver.", "★★★★★ (5-STAR)", null),
            ("Limited Ver.", "★★★★★ (5-STAR)", null),

            // BIBI
            ("Standard Ver.", "Lowlife Princess: Noir", null),

            // B.I
            ("Waterfall Ver.", "WATERFALL", null),
            ("Limited Ver.", "WATERFALL", null),

            ("Cosmos Ver.", "COSMOS", null),
            ("Meta Ver.", "COSMOS", null),

            ("TO Ver.", "TO DIE FOR", null),
            ("DIE Ver.", "TO DIE FOR", null),
            ("FOR Ver.", "TO DIE FOR", null),

            // (G)I-DLE - I NEVER DIE
            ("Risky Ver.", "I NEVER DIE", null),
            ("Chill Ver.", "I NEVER DIE", null),
            ("Spoiled Ver.", "I NEVER DIE", null),

            // (G)I-DLE - I FEEL
            ("Cat Ver.", "I FEEL", null),
            ("Butterfly Ver.", "I FEEL", null),
            ("Cool Ver.", "I FEEL", null),

            // TWICE - Formula of Love
            ("Study About Love Ver.", "Formula of Love: O+T=<3", null),
            ("Break It Ver.", "Formula of Love: O+T=<3", null),
            ("Explosion Ver.", "Formula of Love: O+T=<3", null),
            ("Full of Love Ver.", "Formula of Love: O+T=<3", null),

            // TWICE - READY TO BE
            ("A Ver.", "READY TO BE", null),
            ("B Ver.", "READY TO BE", null),
            ("C Ver.", "READY TO BE", null),

            // SHINee - The Story of Light Ep.1
            ("Ep.1 Ver.", "The Story of Light Ep.1", null),

            // SHINee - Don't Call Me
            ("Fake Reality Ver.", "Don't Call Me", null),
            ("Reality Ver.", "Don't Call Me", null),

            // SHINee - HARD
            ("HARD Ver.", "HARD", null),
            ("BRAVE Ver.", "HARD", null),
            ("LUCKY Ver.", "HARD", null),
        ];*/
}