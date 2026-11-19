# Zadanie 3.1 — Minutnik, który spełnia siedem zasad naraz

- **Data pomiaru:** 2026-11-19
- **Narzędzie i model:** Antigravity IDE (model Google Gemini)
- **Tokenizer użyty do pomiaru:** The Tokenizer Playground (`https://huggingface.co/spaces/Xenova/the-tokenizer-playground`)

---

## 1. Cztery liczby z lekcji 3a

1. **Liczba tokenów pierwszej wiadomości po polsku:** **132 tokeny** (w tokenizerze `o200k_base` / **158 tokenów** w `cl100k_base`, dla tekstu o długości 459 znaków / 68 słów):
   > „Zbuduj minutnik w tym projekcie Avalonia. Ma odmierzać 25 minut pracy, potem 5 minut przerwy, i liczyć skończone cykle. Obie długości mają być do zmiany w oknie. Jeden przycisk start/pauza, drugi reset. Czas widoczny jako mm:ss, licznik cykli widoczny obok. Po skończonej pracy przerwa ma ruszyć sama. Interfejs po polsku, nazwy w kodzie po angielsku, bez dodatkowych bibliotek. Cała logika wprost w pliku MainWindow.axaml.cs, bez osobnych klas pośredniczących.”

2. **Liczba tokenów tej samej wiadomości po angielsku:** **94 tokeny** (w tokenizerze `o200k_base` / **96 tokenów** w `cl100k_base`, dla tekstu o długości 492 znaków / 77 słów):
   > "Build a timer in this Avalonia project. It should count down 25 minutes of work, then 5 minutes of break, and count completed cycles. Both durations must be editable in the window. One start/pause button, a second reset button. Time displayed as mm:ss, cycle counter visible next to it. After work finishes, the break must start automatically. Interface in Polish, code identifiers in English, no extra libraries. All logic directly in MainWindow.axaml.cs, without separate intermediate classes."

3. **Okno kontekstu modelu (Context window):** **1 000 000 (1M) tokenów wejścia**
   - Źródło: [Google Gemini API — Models overview](https://ai.google.dev/gemini-api/docs/models) (stan sprawdzony: 2026-11-19).

4. **Maksymalna długość jednej odpowiedzi modelu (Max output tokens):** **64 000 (65 536) tokenów wyjścia**
   - Źródło: [Google Gemini API — Models overview](https://ai.google.dev/gemini-api/docs/models) (stan sprawdzony: 2026-11-19).

---

## 2. Wynik próby wpisania `25,5` oraz `25.5` w pole długości czasu

- **Przed poprawką parsowania (przy zwykłym `int.TryParse`):**
  - Wpisanie `25,5` (z przecinkiem): `int.TryParse` zwraca `false`, ponieważ oczekuje liczby całkowitej bez części ułamkowej — aplikacja odrzuca wartość i przywraca domyślne `25:00`.
  - Wpisanie `25.5` (z kropką): `int.TryParse` również zwraca `false` i aplikacja wraca do domyślnego `25:00` (natomiast przy zwykłym `double.TryParse` w domyślnej polskiej kulturze systemu `pl-PL` wartość `25,5` z przecinkiem zostaje rozpoznana jako 25,5 minuty, a `25.5` z kropką zostaje odrzucone).
- **Po ujednoliceniu separatora dziesiętnego w kodzie (`input.Trim().Replace(',', '.')` + `double.TryParse(..., CultureInfo.InvariantCulture)`):**
  - Wpisanie **`25,5`**: wartość jest poprawnie przeliczana na `25.5 * 60 = 1530` sekund, a wyświetlacz od razu pokazuje **`25:30`**.
  - Wpisanie **`25.5`**: kropka jest również traktowana jako separator dziesiętny (`1530` sekund) i wyświetlacz pokazuje dokładnie **`25:30`**.
